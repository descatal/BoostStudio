using BoostStudio.Domain.Entities.Exvs.Stats;
using BoostStudio.Domain.Entities.Exvs.Units;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Stats;

public class UnitStatsConfiguration : IEntityTypeConfiguration<UnitStat>
{
    public void Configure(EntityTypeBuilder<UnitStat> builder)
    {
        builder.HasOne(unitStat => unitStat.Unit)
            .WithOne(unit => unit.UnitStats)
            .HasForeignKey<UnitStat>(unitStat => unitStat.GameUnitId)
            .HasPrincipalKey<Unit>(unit => unit.GameUnitId)
            .IsRequired(false);
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
