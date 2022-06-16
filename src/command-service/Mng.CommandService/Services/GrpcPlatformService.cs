using MapsterMapper;
using Mng.CommandService.Grpc;
using PlatformModel = Mng.CommandService.Data.Models.Platform;

namespace Mng.CommandService.Services;

public class GrpcPlatformService : IPlatformService
{
    private readonly Platforms.PlatformsClient _grpcClient;
    private readonly IMapper _mapper;

    public GrpcPlatformService(
        Platforms.PlatformsClient grpcClient,
        IMapper mapper
    )
    {
        _grpcClient = grpcClient;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlatformModel>> GetAllAsync()
    {
        var getAllResponse = await _grpcClient.GetAllAsync(new GetAllRequest());
        var platforms = _mapper.Map<IEnumerable<PlatformModel>>(getAllResponse.Data);

        return platforms;
    }
}