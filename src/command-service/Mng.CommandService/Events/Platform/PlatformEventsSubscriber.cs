using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Mng.CommandService.Options;

namespace Mng.CommandService.Events.Platform;

public class PlatformEventsSubscriber : BackgroundService
{
    private readonly IOptions<RabbitMqOptions> _rabbitMqOptions;
    private readonly PlatformEventMessageHandler _platformEventMessageHandler;
    private readonly ILogger<PlatformEventsSubscriber> _logger;
    private IConnection? _connection;
    private IModel? _model;

    private bool _disposed;

    public PlatformEventsSubscriber(
        IOptions<RabbitMqOptions> rabbitMqOptions,
        PlatformEventMessageHandler platformEventMessageHandler,
        ILogger<PlatformEventsSubscriber> logger
    )
    {
        _rabbitMqOptions = rabbitMqOptions;
        _platformEventMessageHandler = platformEventMessageHandler;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var rabbitMqOptions = _rabbitMqOptions.Value;

        var factory = new ConnectionFactory
        {
            HostName = rabbitMqOptions.Host,
            Port = rabbitMqOptions.Port
        };

        _connection = factory.CreateConnection();
        _model = _connection.CreateModel();

        // TODO: Refactor
        _model.ExchangeDeclare("trigger", ExchangeType.Fanout);

        var queueName = _model.QueueDeclare().QueueName;
        
        _model.QueueBind(queueName, "trigger", "");

        _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;

        var consumer = new EventingBasicConsumer(_model);

        consumer.Received += async (sender, args) =>
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body.Span);

            await _platformEventMessageHandler.HandleMessageAsync(message);
        };

        _model.BasicConsume(consumer, queueName, true);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        Dispose(true);
        base.Dispose();
        
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _connection?.Dispose();
            _model?.Dispose();

            _connection = null;
            _model = null;
        }

        _disposed = true;
    }
    
    private void RabbitMq_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation("RabbitMq connection closed");
    }
}