using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;
using UnitMapper=BoostStudio.Application.Contracts.Mappers.UnitMapper;

namespace BoostStudio.Application.Exvs.Units.Queries;

public record GetUnitQuery(
    uint[]? GameUnitIds = null
) : IRequest<List<UnitDto>>;

public class GetUnitQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitQuery, List<UnitDto>>
{
    public async Task<List<UnitDto>> Handle(GetUnitQuery query, CancellationToken cancellationToken)
    {
        var unitsQuery = applicationDbContext.Units.AsQueryable();

        if (query.GameUnitIds is not null && query.GameUnitIds.Length != 0)
            unitsQuery = unitsQuery.Where(unit => query.GameUnitIds.Contains(unit.GameUnitId));

        var unit = await unitsQuery.ToListAsync(cancellationToken);
        var mapper = new UnitMapper();
        
        return mapper.UnitToUnitDto(unit);
    }
}
