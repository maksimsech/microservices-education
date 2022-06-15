using MapsterMapper;
using Mng.CommandService.Data;
using Mng.CommandService.DataContracts.PlatformService;
using PlatformModel = Mng.CommandService.Data.Models.Platform;

namespace Mng.CommandService.Events.Platform;

public class PlatformEventHandler
{
    private readonly CommandContext _context;
    private readonly IMapper _mapper;

    public PlatformEventHandler(
        CommandContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task HandlePlatformPublished(PlatformPublishedDataContract platformPublished)
    {
        var platform = _mapper.Map<PlatformModel>(platformPublished);

        _context.Add(platform);

        await _context.SaveChangesAsync();
    }
}