using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Projectiles;

public class ProjectileConfiguration: IEntityTypeConfiguration<Projectile>
{
    public void Configure(EntityTypeBuilder<Projectile> builder)
    {
        builder.HasAlternateKey(projectile => projectile.Hash);

        builder.HasOne(projectile => projectile.Hitbox)
            .WithMany()
            .HasForeignKey(projectile => projectile.HitboxHash)
            .HasPrincipalKey(hitbox => hitbox.Hash)
            .IsRequired(false);
        
        builder.HasOne(projectile => projectile.UnitProjectile)
            .WithMany(unitProjectile => unitProjectile.Projectiles)
            .HasForeignKey(projectile => projectile.UnitProjectileId)
            .IsRequired(false);
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}

