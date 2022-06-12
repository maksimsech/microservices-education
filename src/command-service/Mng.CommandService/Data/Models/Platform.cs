namespace Mng.CommandService.Data.Models;

public class Platform
{
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Name { get; set; } = null!;


    public ICollection<Platform> Platforms { get; set; } = null!;
}