﻿using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Formats.AmmoFormat.Commands;

public record SerializeAmmoCommand(string DestinationPath) : IRequest;

public class SerializeAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    IFormatBinarySerializer<List<Ammo>> ammoBinarySerializer,
    ILogger<SerializeAmmoCommandHandler> logger
) : IRequestHandler<SerializeAmmoCommand>
{
    public async ValueTask<Unit> Handle(SerializeAmmoCommand request, CancellationToken cancellationToken)
    {
        var ammo = await applicationDbContext.Ammo.ToListAsync(cancellationToken);
        var serializedBinary = await ammoBinarySerializer.SerializeAsync(ammo, cancellationToken);
        await File.WriteAllBytesAsync(request.DestinationPath, serializedBinary, cancellationToken: cancellationToken);

        return default;
    }
}
