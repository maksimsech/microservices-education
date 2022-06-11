namespace Mng.PlatformService.Options;

public class HttpCommandSyncServiceOptions
{
    public static string SectionName = "HttpCommandSyncService";

    public string BaseAddress { get; init; } = null!;

    public string AddPlatform { get; init; } = null!;
}