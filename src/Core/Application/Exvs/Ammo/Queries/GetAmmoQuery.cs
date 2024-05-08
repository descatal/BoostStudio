using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AmmoMapper=BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Queries;

public record GetAmmoQuery(long[]? Hashes = null) : IRequest<AmmoView>;

public class GetAmmoHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<GetAmmoHandler> logger
) : IRequestHandler<GetAmmoQuery, AmmoView>
{
    public async Task<AmmoView> Handle(GetAmmoQuery request, CancellationToken cancellationToken)
    {
        var mapper = new AmmoMapper();
        var ammoQueryable = applicationDbContext.Ammo.AsQueryable();

        if (request.Hashes is not null)
            ammoQueryable = ammoQueryable.Where(a => request.Hashes.Contains(a.Hash));
        
        var ammo = await ammoQueryable
            .Select(a => mapper.AmmoToAmmoDto(a))
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new AmmoView(ammo);
    }
}
