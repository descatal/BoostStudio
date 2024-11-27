using BoostStudio.Domain.Entities.Exvs.Ammo;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations;

public class AmmoConfiguration : IEntityTypeConfiguration<Ammo>
{
    public void Configure(EntityTypeBuilder<Ammo> builder)
    {
        builder.HasAlternateKey(ammo => ammo.Hash);
        
        builder.HasOne(ammo => ammo.UnitStat)
            .WithMany(unitStat => unitStat.Ammo)
            .HasForeignKey(ammo => ammo.UnitStatId)
            .IsRequired(false);
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
