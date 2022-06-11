using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mng.PlatformService.Data;

public class DesignTimePlatformContextFactory : IDesignTimeDbContextFactory<PlatformContext>
{
    private const string DesignTimeConnectionString = "Server=(localdb)\\mssqllocaldb;Database=PlatformService;Trusted_Connection=True;MultipleActiveResultSets=true";

    public PlatformContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<PlatformContext>()
            .UseSqlServer(DesignTimeConnectionString).Options;

        return new PlatformContext(options);
    }
}