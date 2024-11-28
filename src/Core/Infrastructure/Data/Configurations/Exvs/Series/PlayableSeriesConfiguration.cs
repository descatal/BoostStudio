using BoostStudio.Domain.Entities.Exvs.Series;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Series;

public class TblConfiguration : IEntityTypeConfiguration<PlayableSeries>
{
    public void Configure(EntityTypeBuilder<PlayableSeries> builder)
    {
        builder.HasAlternateKey(series => series.InGameId);

        builder.HasOne(series => series.MovieAsset)
            .WithOne()
            .HasForeignKey<PlayableSeries>(series => series.MovieAssetHash)
            .IsRequired(false);
    }
}
