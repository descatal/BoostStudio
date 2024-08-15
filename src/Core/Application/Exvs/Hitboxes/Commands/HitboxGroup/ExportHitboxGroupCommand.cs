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

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record ExportHitboxGroupCommand(uint[]? Hashes = null, uint[]? UnitIds = null) : IRequest<FileInfo>;

public record ExportHitboxGroupByPathCommand(uint[]? Hashes = null, uint[]? UnitIds = null, string? OutputPath = null) : IRequest;

public class ExportHitboxGroupCommandHandler(
    IHitboxGroupBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    ICompressor compressor,
    ILogger<ExportHitboxGroupCommandHandler> logger
) : IRequestHandler<ExportHitboxGroupCommand, FileInfo>,
    IRequestHandler<ExportHitboxGroupByPathCommand>
{
    public async ValueTask<FileInfo> Handle(ExportHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        var generatedBinaries = await GenerateBinary(command.Hashes, command.UnitIds, cancellationToken);
        
        var tarFileBytes = await compressor.CompressAsync(generatedBinaries, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "hitbox.tar", MediaTypeNames.Application.Octet);
    }
    
    public async ValueTask<Unit> Handle(ExportHitboxGroupByPathCommand command, CancellationToken cancellationToken)
    {
        var generatedBinaries = await GenerateBinary(command.Hashes, command.UnitIds, cancellationToken);
        
        var exportPath = command.OutputPath ?? string.Empty;
        if (string.IsNullOrWhiteSpace(command.OutputPath))
        {
            var configPath = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
            Guard.Against.NotFound(ConfigKeys.WorkingDirectory, configPath.Value);

            exportPath = Path.Combine(configPath.Value.Value, "Hitboxes");
        }
        
        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);
        
        foreach (var binary in generatedBinaries)
        {
            var filePath = Path.Combine(exportPath, $"{binary.FileName}");
            await File.WriteAllBytesAsync(filePath, binary.Data, cancellationToken);
        }
        
        return Unit.Value;
    }

    private async ValueTask<List<FileInfo>> GenerateBinary(
        uint[]? hashes = null,
        uint[]? unitIds = null, 
        CancellationToken cancellationToken = default)
    {
        var query = applicationDbContext.HitboxGroups
            .Include(group => group.Units)
            .Include(group => group.Hitboxes)
            .AsQueryable();

        if (unitIds is not null)
            query = query.Where(group => group.Units.Any(unit => unitIds.Contains(unit.GameUnitId)));
        
        if (hashes is not null)
            query = query.Where(group => hashes.Contains(group.Hash));
            
        var group = await query.ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var hitboxGroup in group)
        {
            var serializedBytes = await binarySerializer.SerializeAsync(hitboxGroup, cancellationToken);
            
            var name = hitboxGroup.Units.Count > 0 
                ? string.Join("-", hitboxGroup.Units.Select(unit => unit.Name)) 
                : hitboxGroup.Hash.ToString();
            
            var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(name);
            fileName = Path.ChangeExtension(fileName, ".hitbox");
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        return fileInfo;
    }
}
