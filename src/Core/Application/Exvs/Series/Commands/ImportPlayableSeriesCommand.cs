using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Domain.Entities.Exvs.Series;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SeriesInfo = BoostStudio.Formats.ListInfoBinaryFormat.SeriesInfo;

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

        var playableSeriesEntities = await applicationDbContext.PlayableSeries
            .Where(entity => allBinarySeriesId.Contains(entity.Id))
            .ToListAsync(cancellationToken);

        foreach (var binarySeriesInfo in allBinarySeriesInfo)
        {
            if (binarySeriesInfo is null)
                continue;

            var binaryEntity = PlayableSeriesMapper.MapToEntity(binarySeriesInfo);
            var playableSeriesEntity = playableSeriesEntities.FirstOrDefault(playableSeries => playableSeries.Id == binarySeriesInfo.SeriesId);

            if (playableSeriesEntity is null)
            {
                playableSeriesEntity = new Domain.Entities.Exvs.Series.Series
                {
                    Id = binarySeriesInfo.SeriesId
                };
                await applicationDbContext.PlayableSeries.AddAsync(playableSeriesEntity, cancellationToken);
            }

            PlayableSeriesMapper.UpdateEntity(binaryEntity, playableSeriesEntity);

            if (playableSeriesEntity.MovieAssetHash == 0)
                playableSeriesEntity.MovieAssetHash = null;

            var seriesMetadata = seriesMetadataOptions.Value.FirstOrDefault(option => binaryEntity.Id == option.Id);
            if (seriesMetadata is not null)
                PlayableSeriesMapper.UpdateEntityMetadata(seriesMetadata, playableSeriesEntity);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}
