using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Hitboxes;

public class HitboxConfiguration : IEntityTypeConfiguration<Hitbox>
{
    public void Configure(EntityTypeBuilder<Hitbox> builder)
    {
        builder.HasAlternateKey(projectile => projectile.Hash);
        
        builder.HasOne(hitbox => hitbox.HitboxGroup)
            .WithMany(group => group.Hitboxes)
            .HasForeignKey(hitbox => hitbox.HitboxGroupHash)
            .HasPrincipalKey(group => group.Hash)
            .IsRequired();
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
