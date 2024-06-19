using System.Text.Json;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;

public record ExportUnitProjectileByIdCommand(uint UnitId) : IRequest<FileInfo>;

public class ExportUnitProjectileByIdCommandHandler(
    IUnitProjectileBinarySerializer statBinarySerializer,
    IApplicationDbContext applicationDbContext,
    ICompressor compressor,
    ILogger<ExportUnitProjectileByIdCommandHandler> logger
) : IRequestHandler<ExportUnitProjectileByIdCommand, FileInfo>
{
    public async Task<FileInfo> Handle(ExportUnitProjectileByIdCommand command, CancellationToken cancellationToken)
    {
        var unitProjectile = await applicationDbContext.UnitProjectiles
            .Include(projectile => projectile.Unit)
            .Where(projectile => command.UnitId == projectile.GameUnitId)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.UnitId, unitProjectile);
        
        var serializedBytes = await statBinarySerializer.SerializeAsync(unitProjectile, cancellationToken);
        var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(unitProjectile.Unit?.Name ?? unitProjectile.GameUnitId.ToString());
        fileName = Path.ChangeExtension(fileName, ".stats");
        return new FileInfo(serializedBytes, fileName);
    }
}
