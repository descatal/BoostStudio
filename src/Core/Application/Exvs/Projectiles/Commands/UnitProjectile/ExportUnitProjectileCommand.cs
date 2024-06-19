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
    IUnitProjectileBinarySerializer statBinarySerializer,
    IApplicationDbContext applicationDbContext,
    ICompressor compressor,
    ILogger<ExportUnitProjectileCommandHandler> logger
) : IRequestHandler<ExportUnitProjectileCommand, FileInfo>
{
    public async Task<FileInfo> Handle(ExportUnitProjectileCommand command, CancellationToken cancellationToken)
    {
        var unitProjectiles = await applicationDbContext.UnitProjectiles
            .Include(stat => stat.Unit)
            .Where(stat => command.UnitIds.Contains(stat.GameUnitId))
            .ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var stat in unitProjectiles)
        {
            var serializedBytes = await statBinarySerializer.SerializeAsync(stat, cancellationToken);
            var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(stat.Unit?.Name ?? stat.GameUnitId.ToString());
            fileName = Path.ChangeExtension(fileName, ".stats");
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        var tarFileBytes = await compressor.CompressAsync(fileInfo, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "stats.tar", MediaTypeNames.Application.Octet);
    }
}
