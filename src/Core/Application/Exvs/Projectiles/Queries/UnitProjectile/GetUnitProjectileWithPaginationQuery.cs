using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;
using BoostStudio.Application.Contracts.Projectiles.UnitProjectiles;

namespace BoostStudio.Application.Exvs.Projectiles.Queries.UnitProjectile;

public record GetUnitProjectileWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? UnitIds = null,
    string? Search = null
) : IRequest<PaginatedList<UnitProjectileDto>>;

public class GetUnitProjectileWithPaginationQueryHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetUnitProjectileWithPaginationQuery, PaginatedList<UnitProjectileDto>>
{
    public async ValueTask<PaginatedList<UnitProjectileDto>> Handle(
        GetUnitProjectileWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var unitProjectilesQueryable = applicationDbContext.UnitProjectiles.AsQueryable();

        if (request.UnitIds is not null && request.UnitIds.Length != 0)
        {
            unitProjectilesQueryable = unitProjectilesQueryable.Where(unitProjectile =>
                request.UnitIds.Contains(unitProjectile.GameUnitId)
            );
        }

        var mappedQueryable = UnitProjectileMapper.ProjectToDto(unitProjectilesQueryable);
        var result = await PaginatedList<UnitProjectileDto>.CreateAsync(
            mappedQueryable,
            request.Page,
            request.PerPage
        );

        return result;
    }
}
