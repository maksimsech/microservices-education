namespace Mng.PlatformService.DataContracts;

public class PlatformReadDataContract
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public string Cost { get; set; } = null!;
}