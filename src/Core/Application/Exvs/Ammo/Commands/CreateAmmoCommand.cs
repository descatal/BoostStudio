using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.Extensions.Logging;
using AmmoMapper=BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record CreateAmmoCommand : AmmoDto, IRequest
{
    public uint? AmmoHash { get; set; }
}

public class CreateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<CreateAmmoCommandHandler> logger
) : IRequestHandler<CreateAmmoCommand>
{
    public async Task Handle(CreateAmmoCommand request, CancellationToken cancellationToken)
    {
        // Generate a random hash if not supplied
        request.AmmoHash ??= BinaryPrimitives.ReadUInt32BigEndian(Crc32.Hash(Encoding.Default.GetBytes(Guid.NewGuid().ToString())));
        
        var mapper = new AmmoMapper();
        var ammo = mapper.AmmoDtoToAmmo(request);
        await applicationDbContext.Ammo.AddAsync(ammo, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
