using Grpc.Core;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Mng.PlatformService.Data;

namespace Mng.PlatformService.Grpc;

// TODO: Think about good name for service bcs it looks bad...
public class PlatformService : Platforms.PlatformsBase
{
    private readonly PlatformContext _context;
    private readonly IMapper _mapper;

    public PlatformService(
        PlatformContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async override Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
    {
        var platforms = await _context.Platforms.ToListAsync();
        var grpcPlatforms = _mapper.Map<IEnumerable<Platform>>(platforms);
        var response = new GetAllResponse
        {
            Data = { grpcPlatforms },
        };

        return response;
    }
}