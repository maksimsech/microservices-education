using Microsoft.EntityFrameworkCore;
using Mng.CommandService.Data.Models;
using Mng.CommandService.Services;

namespace Mng.CommandService.Data;

public static class DbInitializer
{
    public static void UseDbInitializer(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CommandContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CommandContext>>();
        var platformService = scope.ServiceProvider.GetRequiredService<IPlatformService>();

        SeedData(context, logger, platformService);
    }

    private static void SeedData(DbContext context, ILogger logger, IPlatformService platformService)
    {
        logger.LogInformation("Begin db initialization");

        SeedPlatforms(context, platformService);

        context.SaveChanges();

        logger.LogInformation("Finish db initialization");
    }

    private static void SeedPlatforms(DbContext context, IPlatformService platformService)
    {
        var platformsSet = context.Set<Platform>();

        if (platformsSet.Any())
        {
            return;
        }

        var platforms = platformService.GetAllAsync().GetAwaiter().GetResult();

        platformsSet.AddRange(platforms);
    }
}