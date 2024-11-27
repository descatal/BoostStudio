using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record ImportAmmoCommand(Stream File) : IRequest;

public class ImportAmmoCommandHandler(
    IFormatBinarySerializer<List<Domain.Entities.Exvs.Ammo.Ammo>> ammoBinarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ImportAmmoCommandHandler> logger
) : IRequestHandler<ImportAmmoCommand>
{
    public async ValueTask<Unit> Handle(ImportAmmoCommand command, CancellationToken cancellationToken)
    {
        var deserializedAmmoList = await ammoBinarySerializer.DeserializeAsync(command.File, cancellationToken);
        var deserializedAmmoHashes = deserializedAmmoList.Select(ammo => ammo.Hash).ToList();
        
        // Does upsert on the ammo
        var existingAmmo = await applicationDbContext.Ammo
            .Where(ammo => deserializedAmmoHashes.Contains(ammo.Hash))
            .ToDictionaryAsync(ammo => ammo.Hash, cancellationToken);

        foreach (var deserializedAmmo in deserializedAmmoList)
        {
            if (existingAmmo.TryGetValue(deserializedAmmo.Hash, out var queriedAmmo))
            {
                queriedAmmo = deserializedAmmo;
                continue;
            }

            applicationDbContext.Ammo.Add(deserializedAmmo);
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return default;
    }
}
