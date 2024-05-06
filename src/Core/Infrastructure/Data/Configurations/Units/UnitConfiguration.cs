using BoostStudio.Domain.Entities.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Units;

public class UnitConfiguration: IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasOne(unit => unit.UnitStats)
            .WithOne(unitStat => unitStat.Unit)
            .HasForeignKey<UnitStat>(unitStat => unitStat.GameUnitId)
            .HasPrincipalKey<Unit>(unit => unit.GameUnitId)
            .IsRequired(false);
    }
}
