using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Exvs.Ammo.Mappers;
using BoostStudio.Application.Exvs.Ammo.Models;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record BulkCreateAmmoCommand(List<CreateAmmoDto> Ammo) : IRequest;

public class BulkCreateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    ILogger<BulkCreateAmmoCommandHandler> logger
) : IRequestHandler<BulkCreateAmmoCommand>
{
    public async Task Handle(BulkCreateAmmoCommand request, CancellationToken cancellationToken)
    {
        request.Ammo.ForEach(dto =>
        {
            dto.Hash ??= BinaryPrimitives.ReadUInt32BigEndian(Crc32.Hash(Encoding.Default.GetBytes(Guid.NewGuid().ToString())));
        });
        
        var mapper = new AmmoMapper();
        var ammo = mapper.CreateAmmoDtoToAmmo(request.Ammo);
        await applicationDbContext.Ammo.AddRangeAsync(ammo, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
