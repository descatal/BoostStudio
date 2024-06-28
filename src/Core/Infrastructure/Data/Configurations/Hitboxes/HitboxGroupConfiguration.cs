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
        
        builder.HasOne(group => group.Unit)
            .WithOne(unit => unit.HitboxGroup)
            .HasForeignKey<HitboxGroup>(unitStat => unitStat.GameUnitId)
            .HasPrincipalKey<Unit>(unit => unit.GameUnitId)
            .IsRequired(false);
    }
}
