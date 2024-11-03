using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record ResizePatchFileCommand(
    Guid[]? Ids = null,
    PatchFileVersion[]? Versions = null,
    uint[]? UnitIds = null,
    AssetFileType[]? AssetFileTypes = null
) : IRequest;

public class ResizePatchFileCommandHandler(
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository
) : IRequestHandler<ResizePatchFileCommand>
{
    public async ValueTask<Unit> Handle(ResizePatchFileCommand request, CancellationToken cancellationToken)
    {
        var stagingDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.StagingDirectory, cancellationToken);
        if (stagingDirectoryConfig.IsError || !Directory.Exists(stagingDirectoryConfig.Value.Value))
            throw new NotFoundException(ConfigKeys.StagingDirectory, stagingDirectoryConfig.FirstError.Description);
        
        var query = applicationDbContext.PatchFiles
            .Include(patchFile => patchFile.AssetFile)
            .ThenInclude(assetFile => assetFile!.Units)
            .AsQueryable();

        if (request.Ids?.Length > 0)
            query = query.Where(patchFile => request.Ids.Contains(patchFile.Id));
        
        if (request.Versions?.Length > 0)
            query = query.Where(patchFile => request.Versions.Contains(patchFile.TblId));

        if (request.UnitIds?.Length > 0)
        {
            query = query.Where(patchFile => 
                patchFile.AssetFile != null && 
                request.UnitIds.Any(unitId => patchFile.AssetFile.Units.Any(unit => unit.GameUnitId == unitId))
            );
        }

        if (request.AssetFileTypes?.Length > 0)
        {
            query = query.Where(patchFile => 
                patchFile.AssetFile != null && 
                request.AssetFileTypes.Contains(patchFile.AssetFile.FileType)
            );
        }

        var patchFiles = await query.ToListAsync(cancellationToken);
        foreach (var patchFile in patchFiles)
        {
            if (patchFile.AssetFile is null)
                continue;
            
            if (patchFile.AssetFile.Hash == 0x81C0B90)
            {
                
            }
            
            var destinationBaseDirectory = Path.Combine(
                stagingDirectoryConfig.Value.Value, 
                "psarc", 
                patchFile.TblId.GetPatchName()
            );

            var fileCandidates = Directory.GetFiles(destinationBaseDirectory, $"PATCH{patchFile.AssetFile.Hash:X8}.PAC", SearchOption.AllDirectories);
            var assetFilePath = fileCandidates.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(assetFilePath) || !File.Exists(assetFilePath))
                continue;

            var fileSize = (ulong)(new FileInfo(assetFilePath).Length);
            if (fileSize <= 0)
                continue;

            // update the entry's size if found
            patchFile.FileInfo ??= new PatchFileInfo();
            patchFile.FileInfo.Size1 = fileSize;
            patchFile.FileInfo.Size2 = fileSize;
            patchFile.FileInfo.Size3 = fileSize;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}

