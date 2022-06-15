namespace Mng.CommandService.Options;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";


    public string Host { get; init; } = null!;

    public int Port { get; init; }
}