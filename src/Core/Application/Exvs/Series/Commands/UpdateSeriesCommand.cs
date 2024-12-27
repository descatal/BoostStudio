using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Series;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs.Series.Commands;

public record UpdateSeriesCommand : PlayableSeriesDto, IRequest;

public class UpdateSeriesCommandHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<UpdateSeriesCommandHandler> logger
) : IRequestHandler<UpdateSeriesCommand>
{
    public async ValueTask<Unit> Handle(UpdateSeriesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a new playable series...");

        // var existingEntity = applicationDbContext.PlayableSeries
        //     .FirstOrDefault(series => series.Id == request.Id);
        //
        // Guard.Against.NotFound(request.Id.ToString(), existingEntity);
        //
        // PlayableSeriesMapper.UpdateEntity(request, existingEntity);
        // await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
