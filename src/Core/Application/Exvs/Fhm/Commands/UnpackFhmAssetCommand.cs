using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Formats.FhmFormat.Commands;
using BoostStudio.Domain.Enums;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Fhm.Commands;

public record UnpackFhmAssetCommand(
    AssetFileType Type,
    uint[]? UnitIds = null,
    PatchFileVersion? Version = null,
    bool ReplaceWorking = false
) : IRequest<FileInfo>;

public class UnpackFhmAssetCommandHandler(
    IMediator mediator,
    ICompressor compressor,
    IConfigsRepository configsRepository,
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UnpackFhmAssetCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(UnpackFhmAssetCommand request, CancellationToken cancellationToken)
    {
        var isUnitSpecific = request.Type.IsUnitSpecific();
        
        if (isUnitSpecific && (request.UnitIds is null || request.UnitIds.Length <= 0))
            throw new Common.Exceptions.ValidationException([new ValidationFailure("Type", $"Type {request.Type} must have an associated unit id!")]);
        
        var workingDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
        if (workingDirectoryConfig.IsError)
            throw new NotFoundException(ConfigKeys.WorkingDirectory, workingDirectoryConfig.FirstError.Description);

        var stagingDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.StagingDirectory, cancellationToken);
        if (stagingDirectoryConfig.IsError)
            throw new NotFoundException(ConfigKeys.StagingDirectory, stagingDirectoryConfig.FirstError.Description);

        var assetFilesQuery = applicationDbContext.AssetFiles
            .Include(assetFile => assetFile.PatchFiles)
            .Include(assetFile => assetFile.Units)
            .AsQueryable();

        assetFilesQuery = assetFilesQuery.Where(assetFile => assetFile.FileType == request.Type);

        if (isUnitSpecific && request.UnitIds?.Length > 0)
        {
            assetFilesQuery = assetFilesQuery
                .Where(assetFile => assetFile.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId)));
        }

        var assetFiles = await assetFilesQuery
            .ToListAsync(cancellationToken);
        
        var packedFiles = new List<FileInfo>();
        foreach (var assetFile in assetFiles)
        {
            if (request.UnitIds is not null && assetFile.FileType.IsUnitSpecific())
            {
                foreach (var unit in assetFile.Units)
                {
                    if (!request.UnitIds.Contains(unit.GameUnitId))
                        continue;

                    var patchFileInfo = request.Version is not null
                        ? assetFile.PatchFiles.FirstOrDefault(file => file.TblId == request.Version)
                        : assetFile.PatchFiles
                            .OrderBy(file => file.TblId)
                            .LastOrDefault();

                    if (patchFileInfo?.AssetFile is null)
                        continue;

                    var tblName = patchFileInfo.TblId.GetPatchName();
                    var fileTypeName = assetFile.FileType.GetSnakeCaseName();

                    // todo: these are needed for current psarc directory structure
                    string sourceDirectory = string.Empty;
                    var sourceBaseDirectory = Path.Combine(stagingDirectoryConfig.Value.Value, "psarc", tblName, "units");
                    string[] alternateDirectories = ["fb_units", "mbon_units", ""];
                    foreach (var alternateDirectory in alternateDirectories)
                    {
                        sourceDirectory = Path.Combine(sourceBaseDirectory, alternateDirectory, unit.SnakeCaseName, fileTypeName);
                        if (Directory.Exists(sourceDirectory))
                            break;
                    }
                    
                    if (!Directory.Exists(sourceDirectory))
                        throw new NotFoundException(nameof(sourceDirectory), sourceDirectory);
                    
                    var destinationDirectory = request.ReplaceWorking 
                        ? Path.Combine(workingDirectoryConfig.Value.Value, "units", unit.SnakeCaseName, fileTypeName)
                        : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                    try
                    {
                        if (!Directory.Exists(destinationDirectory))
                            Directory.CreateDirectory(destinationDirectory);

                        var sourceFilePath = Path.Combine(sourceDirectory, $"PATCH{assetFile.Hash:X8}.PAC");
                        
                        var packedFile = await mediator.Send(new UnpackFhmPath(sourceFilePath, destinationDirectory), cancellationToken);
                        packedFiles.Add(packedFile);
                    }
                    finally
                    {
                        if (!request.ReplaceWorking)
                            Directory.Delete(destinationDirectory, true);
                    }
                }
            }
            else
            {
                var patchFileInfo = request.Version is not null
                    ? assetFile.PatchFiles.FirstOrDefault(file => file.TblId == request.Version)
                    : assetFile.PatchFiles
                        .OrderBy(file => file.TblId)
                        .FirstOrDefault();

                if (patchFileInfo?.AssetFile is null)
                    continue;

                var tblName = patchFileInfo.TblId.GetPatchName();
                var fileTypeName = assetFile.FileType.GetSnakeCaseName();

                var sourceDirectory = Path.Combine(stagingDirectoryConfig.Value.Value, "psarc", tblName, "common", fileTypeName);
                
                if (!Directory.Exists(sourceDirectory))
                    throw new NotFoundException(nameof(sourceDirectory), sourceDirectory);

                var destinationDirectory = request.ReplaceWorking 
                    ? Path.Combine(workingDirectoryConfig.Value.Value, "common", fileTypeName)
                    : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                try
                {
                    if (!Directory.Exists(destinationDirectory))
                        Directory.CreateDirectory(destinationDirectory);
                    
                    var sourceFilePath = Path.Combine(sourceDirectory, $"PATCH{assetFile.Hash:X8}.PAC");
                    
                    var packedFile = await mediator.Send(new UnpackFhmPath(sourceFilePath, destinationDirectory), cancellationToken);
                    packedFiles.Add(packedFile);
                }
                finally
                {
                    if (!request.ReplaceWorking)
                        Directory.Delete(destinationDirectory, true);
                }
            }
        }
        
        var tarFileBytes = await compressor.CompressAsync(packedFiles, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, $"{request.Type.GetSnakeCaseName()}.tar", MediaTypeNames.Application.Octet);
    }
}
