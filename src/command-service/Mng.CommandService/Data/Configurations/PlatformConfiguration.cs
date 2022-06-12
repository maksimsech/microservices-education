using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mng.CommandService.Data.Models;

namespace Mng.CommandService.Data.Configurations;

public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.ExternalId).IsUnique();

        builder.Property(p => p.Name).IsRequired();
    }
}