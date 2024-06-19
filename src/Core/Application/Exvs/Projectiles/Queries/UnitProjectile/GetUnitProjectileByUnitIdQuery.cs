using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Projectiles.UnitProjectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Projectiles.Queries.UnitProjectile;

public record GetUnitProjectileByUnitIdQuery(uint UnitId) : IRequest<UnitProjectileDto>;

public class GetUnitProjectileByUnitIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitProjectileByUnitIdQuery, UnitProjectileDto>
{
    public async Task<UnitProjectileDto> Handle(GetUnitProjectileByUnitIdQuery request, CancellationToken cancellationToken)
    {
        var unitProjectilesQueryable = applicationDbContext.UnitProjectiles
            .Where(unitProjectile => request.UnitId == unitProjectile.GameUnitId);
        
        var mappedQueryable = UnitProjectileMapper.ProjectToDto(unitProjectilesQueryable);
        var result = await mappedQueryable.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.UnitId, result);
        
        return result;
    }
}
