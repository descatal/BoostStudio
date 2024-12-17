using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Domain.Entities.Exvs.Series;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BoostStudio.Application.Exvs.Series.Commands;

public record CreateSeriesCommand : PlayableSeriesDto, IRequest;

public class CreateSeriesCommandHandler(
    IApplicationDbContext applicationDbContext,
    IOptions<List<SeriesMetadataOption>> seriesMetadataOptions,
    ILogger<CreateSeriesCommand> logger
) : IRequestHandler<CreateSeriesCommand>
{
    public async ValueTask<Unit> Handle(CreateSeriesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new playable series。。。");

        var entity = PlayableSeriesMapper.MapToEntity(request);
        applicationDbContext.PlayableSeries.Add(entity);

        var seriesMetadata = seriesMetadataOptions.Value.FirstOrDefault(option => entity.Id == option.Id);
        if (seriesMetadata is not null)
            PlayableSeriesMapper.UpdateEntityMetadata(seriesMetadata, entity);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
