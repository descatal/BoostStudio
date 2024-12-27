using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Series;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeriesEntity = BoostStudio.Domain.Entities.Exvs.Series.Series;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Series;

public class SeriesConfiguration : IEntityTypeConfiguration<SeriesEntity>
{
    public void Configure(EntityTypeBuilder<SeriesEntity> builder)
    {
        builder.OwnsOne(series => series.PlayableSeries, od =>
        {
            od.WithOwner(playableSeries => playableSeries.Series);

            od.HasOne<AssetFile>(playableSeries => playableSeries.MovieAsset)
                .WithOne()
                .HasForeignKey<PlayableSeries>(playableSeries => playableSeries.MovieAssetHash)
                .IsRequired(false);
        });
    }
}
