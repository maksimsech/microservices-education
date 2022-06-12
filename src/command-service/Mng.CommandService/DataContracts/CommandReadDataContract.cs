namespace Mng.CommandService.DataContracts;

public class CommandReadDataContract
{
    public Guid Id { get; set; }

    public string HowTo { get; set; } = null!;

    public string CommandLine { get; set; } = null!;

    public Guid PlatformId { get; set; }
}