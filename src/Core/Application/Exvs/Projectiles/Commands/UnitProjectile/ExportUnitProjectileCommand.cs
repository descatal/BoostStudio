using System.Net.Mime;
using System.Text.Json;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;

public record ExportUnitProjectileCommand(uint[] UnitIds) : IRequest<FileInfo>;

public class ExportUnitProjectileCommandHandler(
    IUnitProjectileBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    ICompressor compressor,
    ILogger<ExportUnitProjectileCommandHandler> logger
) : IRequestHandler<ExportUnitProjectileCommand, FileInfo>
{
    public async Task<FileInfo> Handle(ExportUnitProjectileCommand command, CancellationToken cancellationToken)
    {
        var unitProjectiles = await applicationDbContext.UnitProjectiles
            .Include(unitProjectile => unitProjectile.Unit)
            .Include(unitProjectile => unitProjectile.Projectiles)
            .Where(unitProjectile => command.UnitIds.Contains(unitProjectile.GameUnitId))
            .ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var unitProjectile in unitProjectiles)
        {
            var serializedBytes = await binarySerializer.SerializeAsync(unitProjectile, cancellationToken);
            var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(unitProjectile.Unit?.Name ?? unitProjectile.GameUnitId.ToString());
            fileName = Path.ChangeExtension(fileName, ".projectile");
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        var tarFileBytes = await compressor.CompressAsync(fileInfo, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "projectiles.tar", MediaTypeNames.Application.Octet);
    }
}
