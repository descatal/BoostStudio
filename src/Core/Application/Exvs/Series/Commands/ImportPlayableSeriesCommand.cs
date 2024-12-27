using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SeriesInfo = BoostStudio.Formats.ListInfoBinaryFormat.SeriesInfo;
using SeriesEntity = BoostStudio.Domain.Entities.Exvs.Series.Series;

namespace BoostStudio.Application.Exvs.Series.Commands;

public record ImportPlayableSeriesCommand(Stream File) : IRequest;

public class ImportPlayableSeriesCommandHandler(
    IListInfoBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    IOptions<List<SeriesMetadataOption>> seriesMetadataOptions,
    ILogger<ImportPlayableSeriesCommandHandler> logger
) : IRequestHandler<ImportPlayableSeriesCommand>
{
    public async ValueTask<Unit> Handle(ImportPlayableSeriesCommand command, CancellationToken cancellationToken)
    {
        var binaryData = await binarySerializer.DeserializeAsync(command.File, cancellationToken);

        if (binaryData.Body.FirstOrDefault() is not SeriesInfo)
            throw new Exception("The file supplied is not a valid series info file format");

        var allBinarySeriesInfo = binaryData.Body
            .Select(kaitaiStruct => kaitaiStruct as SeriesInfo)
            .ToList();

        var allBinarySeriesId = allBinarySeriesInfo
            .Where(info => info is not null)
            .Select(info => info!.SeriesId)
            .ToList();

        var allBinaryAssetHashes = allBinarySeriesInfo
            .Where(info => info is not null)
            .Select(info => info!.MovieAssetHash)
            .ToList();

        var seriesEntities = await applicationDbContext.Series
            .Include(series => series.PlayableSeries)
            .Where(entity => allBinarySeriesId.Contains(entity.Id))
            .ToListAsync(cancellationToken);

        var assetEntities = await applicationDbContext.AssetFiles
            .Where(entity => allBinaryAssetHashes.Contains(entity.Hash))
            .ToListAsync(cancellationToken);

        foreach (var binarySeriesInfo in allBinarySeriesInfo)
        {
            if (binarySeriesInfo is null)
                continue;

            var binaryEntity = PlayableSeriesMapper.MapToEntity(binarySeriesInfo);
            var seriesEntity = seriesEntities.FirstOrDefault(series => series.Id == binarySeriesInfo.SeriesId);

            if (seriesEntity is null)
            {
                seriesEntity = new SeriesEntity
                {
                    Id = binarySeriesInfo.SeriesId
                };
                await applicationDbContext.Series.AddAsync(seriesEntity, cancellationToken);
            }
            seriesEntity.PlayableSeries = binaryEntity;

            // add or update movie asset file if supplied / not 0
            if (binaryEntity.MovieAssetHash != null && binaryEntity.MovieAssetHash != 0)
            {
                var movieAsset = assetEntities.FirstOrDefault(assetFile => assetFile.Hash == seriesEntity.PlayableSeries?.MovieAssetHash);
                if (movieAsset is null)
                {
                    movieAsset = new AssetFile()
                    {
                        Hash = (uint)binaryEntity.MovieAssetHash
                    };
                    applicationDbContext.AssetFiles.Add(movieAsset);
                }
                movieAsset.FileType = AssetFileType.Movie;
                seriesEntity.PlayableSeries!.MovieAsset = movieAsset;
            }

            var seriesMetadata = seriesMetadataOptions.Value.FirstOrDefault(option => binaryEntity.SeriesId == option.Id);
            if (seriesMetadata is not null)
                SeriesMapper.UpdateEntityDetailsIfNull(seriesMetadata, seriesEntity);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}
