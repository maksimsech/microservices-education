using Mapster;
using MapsterMapper;
using Mng.PlatformService.Data.Models;
using Mng.PlatformService.DataContracts;
using GrpcPlatform = Mng.PlatformService.Grpc.Platform;

namespace Mng.PlatformService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapser(this IServiceCollection serviceCollection)
    {
        var config = new TypeAdapterConfig();

        // Verify if it how it should look
        config.NewConfig<Platform, PlatformCreateDataContract>();
        config.NewConfig<Platform, PlatformReadDataContract>();
        config.NewConfig<Platform, PlatformPublishedDataContract>();
        config.NewConfig<Platform, GrpcPlatform>();

        serviceCollection.AddSingleton(config);
        serviceCollection.AddScoped<IMapper, ServiceMapper>();

        return serviceCollection;
    }
}