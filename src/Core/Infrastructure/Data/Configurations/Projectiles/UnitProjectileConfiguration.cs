using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Projectiles;

public class UnitProjectileConfiguration: IEntityTypeConfiguration<UnitProjectile>
{
    public void Configure(EntityTypeBuilder<UnitProjectile> builder)
    {
        builder.HasOne(unitProjectile => unitProjectile.Unit)
            .WithOne(projectile => projectile.UnitProjectiles)
            .HasForeignKey<UnitProjectile>(unitStat => unitStat.GameUnitId)
            .HasPrincipalKey<Unit>(unit => unit.GameUnitId)
            .IsRequired(false);
    }
}

