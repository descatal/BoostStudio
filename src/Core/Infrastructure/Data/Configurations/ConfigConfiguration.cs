using BoostStudio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations;

public class ConfigConfiguration : IEntityTypeConfiguration<Config>
{
    public void Configure(EntityTypeBuilder<Config> builder)
    {
        builder.Ignore(t => t.Id);
        builder.HasKey(t => t.Key);
        builder.Property(t => t.Key).HasMaxLength(100);
        builder.Property(t => t.Value).IsRequired();
    }
}

