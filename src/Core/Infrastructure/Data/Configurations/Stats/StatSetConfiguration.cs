using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Stats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Stats;

public class StatSetConfiguration: IEntityTypeConfiguration<StatSet>
{
    public void Configure(EntityTypeBuilder<StatSet> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(e => e.Unit)
            .WithMany()
            .HasForeignKey(e => e.UnitId)
            .IsRequired();

        builder.HasMany(s => s.Stat)
            .WithOne()
            .IsRequired();
    }
}
