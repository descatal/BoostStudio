using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Queries.Hitbox;

public record GetHitboxByHashQuery(uint Hash) : IRequest<HitboxDto>;

public class GetHitboxByHashQueryHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetHitboxByHashQuery, HitboxDto>
{
    public async ValueTask<HitboxDto> Handle(
        GetHitboxByHashQuery request,
        CancellationToken cancellationToken
    )
    {
        var projectiles = applicationDbContext.Hitboxes.Where(projectile =>
            projectile.Hash == request.Hash
        );

        var queryableDto = HitboxMapper.ProjectToDto(projectiles);
        var dto = await queryableDto.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.Hash, dto);

        return dto;
    }
}
