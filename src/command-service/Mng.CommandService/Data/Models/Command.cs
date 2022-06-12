namespace Mng.CommandService.Data.Models;

public class Command
{
    public Guid Id { get; set; }

    public string HowTo { get; set; } = null!;

    public string CommandLine { get; set; } = null!;

    public Guid PlatformId { get; set; }


    public Platform Platform { get; set; } = null!;
}