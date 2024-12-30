using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BoostStudio.Application.Exvs.Series.Commands;

public record CreateSeriesCommand : SeriesDto, IRequest;

public class CreateSeriesCommandHandler(
    IApplicationDbContext applicationDbContext,
    IOptions<List<SeriesMetadataOption>> seriesMetadataOptions,
    ILogger<CreateSeriesCommand> logger
) : IRequestHandler<CreateSeriesCommand>
{
    public async ValueTask<Unit> Handle(CreateSeriesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new playable series...");

        var entity = SeriesMapper.MapToEntity(request);
        applicationDbContext.Series.Add(entity);

        var seriesMetadata = seriesMetadataOptions.Value.FirstOrDefault(option => entity.Id == option.Id);
        if (seriesMetadata is not null)
            SeriesMapper.UpdateEntityDetailsIfNull(seriesMetadata, entity);

        // add or update movie asset file if supplied / not 0
        if (request.PlayableSeries?.MovieAssetHash != null && request.PlayableSeries.MovieAssetHash != 0)
        {
            var movieAsset = await applicationDbContext.AssetFiles.FirstOrDefaultAsync(
                file => request.PlayableSeries.MovieAssetHash == file.Hash,
                cancellationToken
            );

            if (movieAsset is null)
            {
                movieAsset = new AssetFile()
                {
                    Hash = (uint)request.PlayableSeries.MovieAssetHash,
                };
                applicationDbContext.AssetFiles.Add(movieAsset);
            }

            movieAsset.AddFileType(AssetFileType.Movie);
            entity.PlayableSeries!.MovieAsset = movieAsset;
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
