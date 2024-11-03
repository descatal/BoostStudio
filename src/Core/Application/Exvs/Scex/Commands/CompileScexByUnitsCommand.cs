using System.Net.Mime;
using System.Text.RegularExpressions;
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

public record CompileScexByUnitsCommand(
    uint[] UnitIds,
    bool ReplaceWorking = false,
    bool HotReload = false
) : IRequest<FileInfo>;

public class CompileScexByUnitsCommandHandler(
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    IMediator mediator,
    ICompressor compressor
) : IRequestHandler<CompileScexByUnitsCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(CompileScexByUnitsCommand request, CancellationToken cancellationToken)
    {
        var scriptDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.ScriptDirectory, cancellationToken);
        if (scriptDirectoryConfig.IsError || !Directory.Exists(scriptDirectoryConfig.Value.Value))
            throw new NotFoundException(ConfigKeys.ScriptDirectory, scriptDirectoryConfig.FirstError.Description);
        
        var workingDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
        if (workingDirectoryConfig.IsError)
            throw new NotFoundException(ConfigKeys.WorkingDirectory, workingDirectoryConfig.FirstError.Description);

        var units = await applicationDbContext.Units
            .Where(x => request.UnitIds.Contains(x.GameUnitId))
            .ToListAsync(cancellationToken);
        
        var files = new List<FileInfo>();
        foreach (var unit in units)
        {
            var pathCandidates = Directory.GetFiles(scriptDirectoryConfig.Value.Value, $"*{unit.GameUnitId}*", SearchOption.AllDirectories);
            var sourceFilePath = pathCandidates.FirstOrDefault(path => Regex.IsMatch(path, $@"\b{unit.GameUnitId}\b"));
            
            if (sourceFilePath is null || !File.Exists(sourceFilePath))
                throw new NotFoundException(nameof(sourceFilePath), $"No script file for {unit.Name} found!");
            
            var destinationFilePath = request.ReplaceWorking 
                ? Path.Combine(workingDirectoryConfig.Value.Value, "units", unit.SnakeCaseName, AssetFileType.Data.ToString().ToLower())
                : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                var file = await mediator.Send(new CompileScexByPathCommand(sourceFilePath, destinationFilePath, "005.bin", request.HotReload), cancellationToken);
                files.Add(file);
            }
            finally
            {
                if (request.ReplaceWorking && File.Exists(destinationFilePath))
                    File.Delete(destinationFilePath);
            }
        }
        
        var tarFileBytes = await compressor.CompressAsync(files, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "scex.tar", MediaTypeNames.Application.Octet);            
    }
}
