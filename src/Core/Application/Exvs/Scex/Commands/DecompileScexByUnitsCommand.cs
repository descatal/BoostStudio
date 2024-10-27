using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Scex.Commands;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Scex.Commands;

public record DecompileScexByUnitsCommand(
    uint[] UnitIds,
    bool ReplaceScript = false
) : IRequest<FileInfo>;

public class DecompileScexByUnitsCommandHandler(
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    IMediator mediator,
    ICompressor compressor
) : IRequestHandler<DecompileScexByUnitsCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(DecompileScexByUnitsCommand request, CancellationToken cancellationToken)
    {
        var workingDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
        if (workingDirectoryConfig.IsError)
            throw new NotFoundException(ConfigKeys.WorkingDirectory, workingDirectoryConfig.FirstError.Description);
        
        var scriptDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.ScriptDirectory, cancellationToken);
        if (scriptDirectoryConfig.IsError || !Directory.Exists(scriptDirectoryConfig.Value.Value))
            throw new NotFoundException(ConfigKeys.ScriptDirectory, scriptDirectoryConfig.FirstError.Description);

        var units = await applicationDbContext.Units
            .Where(x => request.UnitIds.Contains(x.GameUnitId))
            .ToListAsync(cancellationToken);
        
        var files = new List<FileInfo>();
        foreach (var unit in units)
        {
            var sourceFilePath = Path.Combine(workingDirectoryConfig.Value.Value, "units", unit.SnakeCaseName, AssetFileType.Data.ToString().ToLower(), "005.bin");
                
            if (!File.Exists(sourceFilePath))
                throw new NotFoundException(nameof(sourceFilePath), $"No compiled script binary for {unit.Name} found!");
            
            var destinationFilePath = request.ReplaceScript
                ? Path.Combine(scriptDirectoryConfig.Value.Value)                 
                : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                var file = await mediator.Send(new DecompileScexByPathCommand(sourceFilePath, destinationFilePath, $"{unit.SnakeCaseName} - {unit.GameUnitId}.c"), cancellationToken);
                files.Add(file);
            }
            finally
            {
                if (request.ReplaceScript && File.Exists(destinationFilePath))
                    File.Delete(destinationFilePath);
            }
        }
        
        var tarFileBytes = await compressor.CompressAsync(files, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "script.tar", MediaTypeNames.Application.Octet);            
    }
}
