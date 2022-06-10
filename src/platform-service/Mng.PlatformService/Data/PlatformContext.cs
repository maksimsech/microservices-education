using Microsoft.EntityFrameworkCore;
using Mng.PlatformService.Data.Configurations;
using Mng.PlatformService.Data.Models;

namespace Mng.PlatformService.Data;

public class PlatformContext : DbContext
{
    public DbSet<Platform> Platforms { get; set; } = null!;


    public PlatformContext(DbContextOptions<PlatformContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PlatformConfiguration());
    }
}