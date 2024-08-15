using System.Net.Mime;
using System.Text.Json;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Stats.Commands.UnitStat;

public record ExportUnitStatCommand(uint[]? UnitIds = null) : IRequest<FileInfo>;

public record ExportUnitStatByPathCommand(uint[]? UnitIds = null, string? ExportPath = null) : IRequest;

public class ExportUnitStatCommandHandler(
    IUnitStatBinarySerializer statBinarySerializer,
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    ICompressor compressor,
    ILogger<ExportUnitStatCommandHandler> logger
) : IRequestHandler<ExportUnitStatCommand, FileInfo>,
    IRequestHandler<ExportUnitStatByPathCommand>
{
    public async ValueTask<FileInfo> Handle(ExportUnitStatCommand command, CancellationToken cancellationToken)
    {
        // return combined tar file
        var generatedBinaries = await GenerateBinary(command.UnitIds, cancellationToken);

        var tarFileBytes = await compressor.CompressAsync(generatedBinaries, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "stats.tar", MediaTypeNames.Application.Octet);
    }
    
    public async ValueTask<Unit> Handle(ExportUnitStatByPathCommand command, CancellationToken cancellationToken)
    {
        // write to specified path
        var generatedBinaries = await GenerateBinary(command.UnitIds, cancellationToken);

        var exportPath = command.ExportPath ?? string.Empty;
        if (string.IsNullOrWhiteSpace(command.ExportPath))
        {
            var configPath = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
            Guard.Against.NotFound(ConfigKeys.WorkingDirectory, configPath.Value);

            exportPath = Path.Combine(configPath.Value.Value, "Stats");
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

    private async ValueTask<List<FileInfo>> GenerateBinary(
        uint[]? unitIds = null, 
        CancellationToken cancellationToken = default)
    {
        var query = applicationDbContext.UnitStats
            .Include(stat => stat.AmmoSlots)
            .Include(stat => stat.Ammo)
            .Include(stat => stat.Stats)
            .Include(stat => stat.Unit)
            .AsQueryable();
            
        if (unitIds is not null)
            query = query.Where(unitProjectile => unitIds.Contains(unitProjectile.GameUnitId));
        
        var unitStats = await query.ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var stat in unitStats)
        {
            if (stat.Unit is null)
                continue;
            
            var serializedBytes = await statBinarySerializer.SerializeAsync(stat, cancellationToken);
            var fileName = Path.ChangeExtension(stat.Unit.SnakeCaseName, ".stats");

            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        return fileInfo;
    }
}
