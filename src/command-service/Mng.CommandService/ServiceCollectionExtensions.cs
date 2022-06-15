using System.Text.Json;
using Mapster;
using MapsterMapper;
using Mng.CommandService.Data.Models;
using Mng.CommandService.DataContracts;
using Mng.CommandService.DataContracts.PlatformService;
using Mng.CommandService.Events.Platform;

namespace Mng.CommandService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapser(this IServiceCollection serviceCollection, Action<TypeAdapterConfig>? configure = null)
    {
        var config = new TypeAdapterConfig();

        // Verify if it how it should look
        config.NewConfig<Platform, PlatformReadDataContract>();
        config.NewConfig<Command, CommandCreateDataContract>();
        config.NewConfig<Command, CommandReadDataContract>();
        config.NewConfig<PlatformPublishedDataContract, Platform>()
            .Map(d => d.ExternalId, s => s.Id)
            .Ignore(s => s.Id);

        configure?.Invoke(config);

        serviceCollection.AddSingleton(config);
        serviceCollection.AddScoped<IMapper, ServiceMapper>();

        return serviceCollection;
    }

    public static IServiceCollection AddPlatformEvents(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<PlatformEventsSubscriber>();
        serviceCollection.AddSingleton<PlatformEventMessageHandler>();
        serviceCollection.AddScoped<PlatformEventHandler>();

        return serviceCollection;
    }
}