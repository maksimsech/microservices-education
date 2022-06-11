using Mng.PlatformService.DataContracts;

namespace Mng.PlatformService.Services.DataSync;

public interface ICommandSyncService
{
    Task SendPlatformAsync(PlatformReadDataContract platform);
}