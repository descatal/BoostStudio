using System.Net.Mime;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AmmoEntity=BoostStudio.Domain.Entities.Unit.Ammo.Ammo;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record ExportAmmoCommand : IRequest<FileInfo>;

public class ExportAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    IFormatBinarySerializer<List<AmmoEntity>> ammoBinarySerializer,
    ILogger<ExportAmmoCommandHandler> logger
) : IRequestHandler<ExportAmmoCommand, FileInfo>
{
    public async Task<FileInfo> Handle(ExportAmmoCommand command, CancellationToken cancellationToken)
    {
        var ammo = await applicationDbContext.Ammo.ToListAsync(cancellationToken);
        var serializedBinary = await ammoBinarySerializer.SerializeAsync(ammo, cancellationToken);
        return new FileInfo(serializedBinary, "all.ammo", MediaTypeNames.Application.Octet);
    }
}
