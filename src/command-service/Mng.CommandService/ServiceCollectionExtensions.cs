using Mapster;
using MapsterMapper;
using Mng.CommandService.Data.Models;
using Mng.CommandService.DataContracts;

namespace Mng.CommandService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapser(this IServiceCollection serviceCollection)
    {
        var config = new TypeAdapterConfig();

        // Verify if it how it should look
        config.NewConfig<Platform, PlatformReadDataContract>();
        config.NewConfig<Command, CommandCreateDataContract>();
        config.NewConfig<Command, CommandReadDataContract>();

        serviceCollection.AddSingleton(config);
        serviceCollection.AddScoped<IMapper, ServiceMapper>();

        return serviceCollection;
    }
}