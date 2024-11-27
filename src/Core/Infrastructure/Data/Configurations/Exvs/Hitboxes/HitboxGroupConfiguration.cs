using BoostStudio.Domain.Entities.Exvs.Hitboxes;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Hitboxes;

public class HitboxGroupConfiguration : IEntityTypeConfiguration<HitboxGroup>
{
    public void Configure(EntityTypeBuilder<HitboxGroup> builder)
    {
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UuiDv7Generator>()
            .ValueGeneratedOnAdd();
        
        builder.HasAlternateKey(projectile => projectile.Hash);
        
        builder.HasMany(group => group.Units)
            .WithOne(unit => unit.HitboxGroup)
            .HasForeignKey(unit => unit.HitboxGroupHash)
            .HasPrincipalKey(group => group.Hash)
            .IsRequired(false);
    }
}
