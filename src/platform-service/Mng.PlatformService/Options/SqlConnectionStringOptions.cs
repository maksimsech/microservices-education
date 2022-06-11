namespace Mng.PlatformService.Options;

public class SqlConnectionStringOptions
{
    public static string SectionName = "SqlConnectionString";

    public string Server { get; init; } = null!;

    public string UserId { get; init; } = null!;

    public string Password { get; init; } = null!;
}