using Mng.PlatformService.DataContracts;

namespace Mng.PlatformService.Services.Events;

public interface IMessageBusService
{
    Task PublishPlatformPublishedAsync(PlatformPublishedDataContract platformPublished);
}