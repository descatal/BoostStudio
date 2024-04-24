using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Exvs.Ammo.Mappers;
using BoostStudio.Application.Exvs.Ammo.Models;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record CreateAmmoCommand : CreateAmmoDto, IRequest;

public class CreateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<CreateAmmoCommandHandler> logger
) : IRequestHandler<CreateAmmoCommand>
{
    public async Task Handle(CreateAmmoCommand request, CancellationToken cancellationToken)
    {
        // Generate a random hash if not supplied
        request.Hash ??= BinaryPrimitives.ReadUInt32BigEndian(Crc32.Hash(Encoding.Default.GetBytes(Guid.NewGuid().ToString())));
        
        var mapper = new AmmoMapper();
        var ammo = mapper.CreateAmmoDtoToAmmo(request);
        await applicationDbContext.Ammo.AddAsync(ammo, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
