using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Hitboxes;

public class UnitHitboxConfiguration : IEntityTypeConfiguration<UnitHitbox>
{
    public void Configure(EntityTypeBuilder<UnitHitbox> builder)
    {
        builder.HasOne(unitHitbox => unitHitbox.Unit)
            .WithOne(unit => unit.UnitHitboxes)
            .HasForeignKey<UnitHitbox>(unitStat => unitStat.GameUnitId)
            .HasPrincipalKey<Unit>(unit => unit.GameUnitId)
            .IsRequired(false);
    }
}
