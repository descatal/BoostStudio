using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;
using UnitMapper=BoostStudio.Application.Contracts.Units.UnitMapper;

namespace BoostStudio.Application.Exvs.Units.Queries;

public record GetUnitQueryByUnitId(uint UnitId) : IRequest<UnitSummaryVm>;

public class GetUnitQueryByUnitIdHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitQueryByUnitId, UnitSummaryVm>
{
    public async ValueTask<UnitSummaryVm> Handle(GetUnitQueryByUnitId query, CancellationToken cancellationToken)
    {
        var unitsQuery = applicationDbContext.Units.AsQueryable();
        var entity = await unitsQuery
            .FirstOrDefaultAsync(unit => query.UnitId == unit.GameUnitId, cancellationToken: cancellationToken);

        Guard.Against.NotFound(query.UnitId, entity);
        return UnitMapper.MapToDto(entity);
    }
}
