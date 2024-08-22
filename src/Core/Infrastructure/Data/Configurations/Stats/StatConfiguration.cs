using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Stats;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Stats;

public class StatConfiguration: IEntityTypeConfiguration<Stat>
{
    public void Configure(EntityTypeBuilder<Stat> builder)
    {
        // A unit can have multiple number of StatSets, with each one representing a new "mode" of the unit
        builder.HasOne(statSet => statSet.UnitStat)
            .WithMany(unitStat => unitStat.Stats)
            .HasForeignKey(statSet => statSet.UnitStatId)
            .IsRequired(false);
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
