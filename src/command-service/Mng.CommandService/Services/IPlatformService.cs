using Mng.CommandService.Data.Models;

namespace Mng.CommandService.Services;

public interface IPlatformService
{
    Task<IEnumerable<Platform>> GetAllAsync();
}