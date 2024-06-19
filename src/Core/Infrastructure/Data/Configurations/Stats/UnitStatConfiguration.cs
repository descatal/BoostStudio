using BoostStudio.Domain.Entities.Unit;
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
    }
}
