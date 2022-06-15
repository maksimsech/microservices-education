using System.Text.Json;
using System.Text.Json.Serialization;
using Mng.CommandService.DataContracts.PlatformService;

namespace Mng.CommandService.Events.Platform;

public class PlatformEventMessageHandler
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PlatformEventMessageHandler> _logger;

    public PlatformEventMessageHandler(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<PlatformEventMessageHandler> logger
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task HandleMessageAsync(string message)
    {
        try
        {
            var (platform, platformEventType) = JsonSerializer.Deserialize<BasicMessageModel>(message, _jsonSerializerOptions)!;

            using var serviceScope = _serviceScopeFactory.CreateScope();
            var platformEventHandler = serviceScope.ServiceProvider.GetRequiredService<PlatformEventHandler>();

            await HandleEventAsync(platformEventHandler, platformEventType, platform.GetRawText());
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Could handle platform event");
        }
    }

    private async Task HandleEventAsync(
        PlatformEventHandler platformEventHandler,
        PlatformEventType eventType,
        string platformString
    )
    {
        var task = eventType switch
        {
            PlatformEventType.Published => platformEventHandler.HandlePlatformPublished(
                JsonSerializer.Deserialize<PlatformPublishedDataContract>(platformString, _jsonSerializerOptions)!
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), "Unknown PlatformEventType"),
        };

        await task;
    }

    private record BasicMessageModel(JsonElement Platform, PlatformEventType Type);
}