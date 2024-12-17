using BoostStudio.Domain.Entities.Exvs.Series;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Series;

public class TblConfiguration : IEntityTypeConfiguration<Domain.Entities.Exvs.Series.Series>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Exvs.Series.Series> builder)
    {
        builder.HasOne(series => series.MovieAsset)
            .WithOne()
            .HasForeignKey<Domain.Entities.Exvs.Series.Series>(series => series.MovieAssetHash)
            .IsRequired(false);
    }
}
