namespace Mng.PlatformService.Data.Models;

public sealed class Platform
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public string Cost { get; set; } = null!;
}