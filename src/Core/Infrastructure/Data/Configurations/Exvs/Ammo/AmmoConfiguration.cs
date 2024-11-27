using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Ammo;

public class AmmoConfiguration : IEntityTypeConfiguration<Domain.Entities.Exvs.Ammo.Ammo>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Exvs.Ammo.Ammo> builder)
    {
        builder.HasAlternateKey(ammo => ammo.Hash);
        
        builder.HasOne(ammo => ammo.UnitStat)
            .WithMany(unitStat => unitStat.Ammo)
            .HasForeignKey(ammo => ammo.UnitStatId)
            .IsRequired(false);
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UuiDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
