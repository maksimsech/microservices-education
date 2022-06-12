using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mng.CommandService.Data.Models;

namespace Mng.CommandService.Data.Configurations;

public class CommandConfiguration : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.HowTo).IsRequired();
        builder.Property(c => c.CommandLine).IsRequired();
    }
}