using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;

public record ExportUnitProjectileCommand(uint[]? UnitIds = null) : IRequest<FileInfo>;

public record ExportUnitProjectileByPathCommand(uint[]? UnitIds = null, string? ExportPath = null) : IRequest;

public class ExportUnitProjectileCommandHandler(
    IUnitProjectileBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    ICompressor compressor,
    ILogger<ExportUnitProjectileCommandHandler> logger
) : IRequestHandler<ExportUnitProjectileCommand, FileInfo>, 
    IRequestHandler<ExportUnitProjectileByPathCommand>
{
    public async ValueTask<FileInfo> Handle(ExportUnitProjectileCommand command, CancellationToken cancellationToken)
    {
        // return combined tar file
        var projectileBinaries = await GenerateBinary(command.UnitIds, cancellationToken);
        
        var tarFileBytes = await compressor.CompressAsync(projectileBinaries, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "projectiles.tar", MediaTypeNames.Application.Octet);
    }
    
    public async ValueTask<Unit> Handle(ExportUnitProjectileByPathCommand command, CancellationToken cancellationToken)
    {
        // write to specified path
        var generatedBinaries = await GenerateBinary(command.UnitIds, cancellationToken);

        var exportPath = command.ExportPath ?? string.Empty;
        if (string.IsNullOrWhiteSpace(command.ExportPath))
        {
            var configPath = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
            Guard.Against.NotFound(ConfigKeys.WorkingDirectory, configPath.Value);

            exportPath = Path.Combine(configPath.Value.Value, "Projectiles");
        }
        
        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);
        
        foreach (var generatedBinary in generatedBinaries)
        {
            var filePath = Path.Combine(exportPath, generatedBinary.FileName);
            await File.WriteAllBytesAsync(filePath, generatedBinary.Data, cancellationToken);
        }

        return Unit.Value;
    }

    private async ValueTask<List<FileInfo>> GenerateBinary(uint[]? unitIds = null, CancellationToken cancellationToken = default)
    {
        var query = applicationDbContext.UnitProjectiles
            .Include(unitProjectile => unitProjectile.Unit)
            .Include(unitProjectile => unitProjectile.Projectiles)
            .AsQueryable();

        if (unitIds is not null)
            query = query.Where(unitProjectile => unitIds.Contains(unitProjectile.GameUnitId));
        
        var unitProjectiles = await query.ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var unitProjectile in unitProjectiles)
        {
            if (unitProjectile.Unit is null)
                continue;
            
            var serializedBytes = await binarySerializer.SerializeAsync(unitProjectile, cancellationToken);
            var fileName = Path.ChangeExtension(unitProjectile.Unit.SnakeCaseName, ".projectile");
            
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        return fileInfo;
    }
}
