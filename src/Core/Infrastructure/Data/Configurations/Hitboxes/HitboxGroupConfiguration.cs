using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Hitboxes;

public class HitboxGroupConfiguration : IEntityTypeConfiguration<HitboxGroup>
{
    public void Configure(EntityTypeBuilder<HitboxGroup> builder)
    {
        builder.HasAlternateKey(projectile => projectile.Hash);
        
        builder.HasMany(group => group.Units)
            .WithOne(unit => unit.HitboxGroup)
            .HasForeignKey(unit => unit.HitboxGroupHash)
            .HasPrincipalKey(group => group.Hash)
            .IsRequired(false);
    }
}
