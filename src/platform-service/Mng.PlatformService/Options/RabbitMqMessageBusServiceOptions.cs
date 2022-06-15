namespace Mng.PlatformService.Options;

public class RabbitMqMessageBusServiceOptions
{
    public const string SectionName = "RabbitMq";

    public string Host { get; init; } = null!;

    public int Port { get; init; }
}