namespace Mng.PlatformService.Options;

public class HttpCommandSyncServiceOptions
{
    public static string SectionName = "HttpCommandSyncService";

    public string BaseAddress { get; init; }

    public string AddPlatform { get; init; }
}