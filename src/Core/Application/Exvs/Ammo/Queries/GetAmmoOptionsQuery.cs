using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Ammo.Queries;

public record GetAmmoOptionsQuery(
    uint[]? UnitIds = null
) : IRequest<List<uint>>;

public class GetAmmoOptionsQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetAmmoOptionsQuery, List<uint>>
{
    public async ValueTask<List<uint>> Handle(GetAmmoOptionsQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Ammo
            .Include(entity => entity.UnitStat)
            .AsQueryable();

        if (request.UnitIds?.Length > 0)
            query = query.Where(ammo => ammo.UnitStat != null && request.UnitIds.Contains(ammo.UnitStat.GameUnitId));
        
        return await query.Select(ammo => ammo.Hash).ToListAsync(cancellationToken);
    }
}
