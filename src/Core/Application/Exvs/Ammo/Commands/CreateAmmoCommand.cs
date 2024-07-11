using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AmmoMapper=BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record CreateAmmoCommand : AmmoDto, IRequest;

public class CreateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<CreateAmmoCommandHandler> logger
) : IRequestHandler<CreateAmmoCommand>
{
    public async ValueTask<Unit> Handle(CreateAmmoCommand request, CancellationToken cancellationToken)
    {
        var ammo = AmmoMapper.AmmoDtoToAmmo(request);
        var unitStat = await applicationDbContext.UnitStats
            .FirstOrDefaultAsync(x => x.GameUnitId == request.UnitId, cancellationToken);

        ammo.UnitStat = unitStat;
        
        await applicationDbContext.Ammo.AddAsync(ammo, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return default;
    }
}
