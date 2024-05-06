using BoostStudio.Domain.Entities.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Units;

public class UnitStatsConfiguration : IEntityTypeConfiguration<UnitStat>
{
    public void Configure(EntityTypeBuilder<UnitStat> builder)
    {
        // A unit can have multiple number of StatSets, with each one representing a new "mode" of the unit
        builder.HasMany(unitStat => unitStat.Stats)
            .WithOne(statSet => statSet.UnitStat)
            .HasForeignKey(statSet => statSet.UnitStatId)
            .IsRequired(false);
        
        // Ammo table itself should not know the context that it is related to, this allow Ammo table to be singularly serialized
        builder.HasMany(unitStat => unitStat.Ammo)
            .WithOne(ammo => ammo.UnitStat)
            .HasForeignKey(ammo => ammo.UnitStatId)
            .IsRequired(false);
    }
}
