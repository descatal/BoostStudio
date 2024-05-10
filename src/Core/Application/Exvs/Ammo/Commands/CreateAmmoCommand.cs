using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.Extensions.Logging;
using AmmoMapper=BoostStudio.Application.Contracts.Mappers.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record CreateAmmoCommand : AmmoDto, IRequest;

public class CreateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<CreateAmmoCommandHandler> logger
) : IRequestHandler<CreateAmmoCommand>
{
    public async Task Handle(CreateAmmoCommand request, CancellationToken cancellationToken)
    {
        var mapper = new AmmoMapper();
        var ammo = mapper.AmmoDtoToAmmo(request);
        await applicationDbContext.Ammo.AddAsync(ammo, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
