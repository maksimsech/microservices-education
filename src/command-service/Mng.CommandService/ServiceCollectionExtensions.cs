using System.Text.Json;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using Mng.CommandService.Data.Models;
using Mng.CommandService.DataContracts;
using Mng.CommandService.DataContracts.PlatformService;
using Mng.CommandService.Events.Platform;
using Mng.CommandService.Grpc;
using Mng.CommandService.Options;
using Mng.CommandService.Services;
using GrpcPlatform = Mng.CommandService.Grpc.Platform;
using Platform = Mng.CommandService.Data.Models.Platform;

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
        config.NewConfig<GrpcPlatform, Platform>()
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

    public static IServiceCollection AddPlatformGrpcClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPlatformService, GrpcPlatformService>();

        serviceCollection.AddGrpcClient<Platforms.PlatformsClient>((services, options) =>
        {
            var platformUri = services.GetRequiredService<IOptions<GrpcOptions>>().Value.Platform;
            options.Address = new Uri(platformUri);
        });

        return serviceCollection;
    }
}