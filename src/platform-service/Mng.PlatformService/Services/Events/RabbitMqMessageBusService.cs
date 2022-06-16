using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Mng.PlatformService.DataContracts;
using Mng.PlatformService.Options;
using RabbitMQ.Client;

namespace Mng.PlatformService.Services.Events;

public class RabbitMqMessageBusService : IMessageBusService, IDisposable
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new (JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly ILogger<RabbitMqMessageBusService> _logger;
    private IConnection _connection;
    private IModel _model;

    private bool _disposed;

    // TODO: Refactor
    public RabbitMqMessageBusService(
        ILogger<RabbitMqMessageBusService> logger,
        IOptions<RabbitMqMessageBusServiceOptions> options
    )
    {
        _logger = logger;
        var options1 = options.Value;
        
        var factory = new ConnectionFactory
        {
            HostName = options1.Host,
            Port = options1.Port,
        };

        _connection = factory.CreateConnection();
        _model = _connection.CreateModel();

        _model.ExchangeDeclare("trigger", ExchangeType.Fanout);

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    public Task PublishPlatformPublishedAsync(PlatformPublishedDataContract platformPublished)
    {
        var model = new BasicMessageModel<PlatformPublishedDataContract>(platformPublished, PlatformEventType.Published);

        Publish(model);

        return Task.CompletedTask;
    }

    private void Publish<TPlatform>(BasicMessageModel<TPlatform> model)
    {
        if (!_connection.IsOpen)
        {
            throw new Exception("Connection is closed.");
        }


        var message = JsonSerializer.Serialize(model, _jsonSerializerOptions);
        var bytes = Encoding.UTF8.GetBytes(message);
        _model.BasicPublish("trigger", "", null, bytes);
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs args)
    {
        _logger.LogWarning("RabbitMQ connection shutdown");
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

            _connection = null!;
            _model = null!;
        }

        _disposed = true;
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private record BasicMessageModel<TPlatform>(TPlatform Platform, PlatformEventType Type);
}