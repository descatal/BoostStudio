using BoostStudio.Domain.Entities.Exvs.Units;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Units;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.Ignore(entity => entity.Id);
        builder.HasKey(entity => entity.GameUnitId);
    }
}
