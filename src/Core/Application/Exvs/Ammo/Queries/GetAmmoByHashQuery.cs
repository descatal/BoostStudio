using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Ammo.Queries;

public record GetAmmoByHashQuery(uint Hash) : IRequest<AmmoDto>;

public class GetAmmoByHashQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetAmmoByHashQuery, AmmoDto>
{
    public async ValueTask<AmmoDto> Handle(GetAmmoByHashQuery request, CancellationToken cancellationToken)
    {
        var ammo = applicationDbContext.Ammo
            .Include(ammo => ammo.UnitStat)
            .Where(ammo => ammo.Hash == request.Hash);

        var queryableDto = AmmoMapper.ProjectToDto(ammo);
        var dto = await queryableDto.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.Hash.ToString(), dto);
        
        return dto;
    }
}
