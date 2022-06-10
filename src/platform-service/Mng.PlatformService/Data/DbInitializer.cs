using Microsoft.EntityFrameworkCore;
using Mng.PlatformService.Data.Models;

namespace Mng.PlatformService.Data;

public static class DbInitializer
{
    public static void UseDbInitializer(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PlatformContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PlatformContext>>();

        // context.Database.Migrate();
        SeedData(context, logger);
    }

    private static void SeedData(DbContext context, ILogger logger)
    {
        logger.LogInformation("Begin db initialization");

        SeedPlatforms(context);

        context.SaveChanges();

        logger.LogInformation("Finish db initialization");
    }

    private static void SeedPlatforms(DbContext context)
    {
        var platformsSet = context.Set<Platform>();

        if (platformsSet.Any())
        {
            return;
        }

        var platforms = new[]
        {
            new Platform
            {
                Name = "Dot Net",
                Publisher = "Microsoft",
                Cost = "Free",
            },
            new Platform
            {
                Name = "SQL Server Express",
                Publisher = "Microsoft",
                Cost = "Free",
            },
            new Platform
            {
                Name = "K8s",
                Publisher = "Cloud comp",
                Cost = "Free",
            },
        };

        platformsSet.AddRange(platforms);
    }
}