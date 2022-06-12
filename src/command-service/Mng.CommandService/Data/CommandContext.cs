using Microsoft.EntityFrameworkCore;
using Mng.CommandService.Data.Configurations;
using Mng.CommandService.Data.Models;

namespace Mng.CommandService.Data;

public class CommandContext : DbContext
{
    public DbSet<Command> Commands { get; init; } = null!;
    public DbSet<Platform> Platforms { get; init; } = null!;


    public CommandContext(DbContextOptions<CommandContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CommandConfiguration());
        modelBuilder.ApplyConfiguration(new PlatformConfiguration());
    }
}