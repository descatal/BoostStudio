using BoostStudio.Domain.Entities.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Units;

public class UnitConfiguration: IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {

    }
}
