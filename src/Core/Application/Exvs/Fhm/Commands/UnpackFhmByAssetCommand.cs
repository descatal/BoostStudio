using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Common.Utils;
using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Domain.Enums;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Fhm.Commands;

public record UnpackFhmByAssetCommand(
    uint[]? AssetFileHashes = null,
    AssetFileType[]? AssetFileTypes = null,
    uint[]? UnitIds = null,
    PatchFileVersion[]? PatchFileVersions = null,
    bool ReplaceWorking = false
) : IRequest<FileInfo>;

public class UnpackFhmAssetCommandHandler(
    IMediator mediator,
    ICompressor compressor,
    IConfigsRepository configsRepository,
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UnpackFhmByAssetCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(
        UnpackFhmByAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var workingDirectoryConfig = await configsRepository.GetConfig(
            ConfigKeys.WorkingDirectory,
            cancellationToken
        );
        if (workingDirectoryConfig.IsError)
            throw new NotFoundException(
                ConfigKeys.WorkingDirectory,
                workingDirectoryConfig.FirstError.Description
            );

        var stagingDirectoryConfig = await configsRepository.GetConfig(
            ConfigKeys.StagingDirectory,
            cancellationToken
        );
        if (stagingDirectoryConfig.IsError)
        {
            throw new NotFoundException(
                ConfigKeys.StagingDirectory,
                stagingDirectoryConfig.FirstError.Description
            );
        }

        var assetFilesQuery = applicationDbContext
            .AssetFiles.Include(assetFile => assetFile.PatchFiles)
            .Include(assetFile => assetFile.Units)
            .AsQueryable();

        if (request.AssetFileHashes?.Length > 0)
            assetFilesQuery = assetFilesQuery.Where(assetFile =>
                request.AssetFileHashes.Contains(assetFile.Hash)
            );

        if (request.AssetFileTypes?.Length > 0)
            assetFilesQuery = assetFilesQuery.Where(entity =>
                request.AssetFileTypes.Any(type => entity.FileType.Contains(type))
            );

        if (request.UnitIds?.Length > 0)
            assetFilesQuery = assetFilesQuery.Where(assetFile =>
                assetFile.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId))
            );

        var assetFiles = await assetFilesQuery.ToListAsync(cancellationToken);

        var packedFiles = new List<FileInfo>();
        foreach (var assetFile in assetFiles)
        {
            foreach (var fileType in assetFile.FileType)
            {
                if (fileType.IsUnitSpecific())
                {
                    foreach (var unit in assetFile.Units)
                    {
                        List<PatchFile> patchFiles = [];
                        if (request.PatchFileVersions?.Length > 0)
                        {
                            patchFiles = assetFile
                                .PatchFiles.Where(file =>
                                    request.PatchFileVersions.Contains(file.TblId)
                                )
                                .ToList();
                        }
                        else
                        {
                            var latestPatchFile = assetFile
                                .PatchFiles.OrderBy(file => file.TblId)
                                .LastOrDefault();
                            if (latestPatchFile is not null)
                                patchFiles = [latestPatchFile];
                        }

                        foreach (var patchFile in patchFiles)
                        {
                            if (patchFile.AssetFile is null)
                                continue;

                            var tblName = patchFile.TblId.GetPatchName();

                            // todo: these are needed for current psarc directory structure
                            var sourceBaseDirectory = Path.Combine(
                                stagingDirectoryConfig.Value.Value,
                                StagingDirectoryConstants.PsarcDirectory,
                                tblName,
                                StagingDirectoryConstants.UnitsDirectory
                            );

                            var allDirectories = Directory.GetDirectories(
                                path: stagingDirectoryConfig.Value.Value,
                                searchPattern: "*",
                                SearchOption.AllDirectories
                            );

                            var candidateDirectories = PathUtils.GetAssetCandidateDirectory(
                                sourceBaseDirectory,
                                fileType,
                                unit
                            );

                            var fallbackDestinationDirectory =
                                candidateDirectories.FirstOrDefault()
                                ?? Path.Combine(
                                    sourceBaseDirectory,
                                    unit.SnakeCaseName,
                                    fileType.GetSnakeCaseName()
                                );

                            var sourceDirectory = allDirectories.FirstOrDefault(
                                directory =>
                                    candidateDirectories.Any(candidate =>
                                        candidate.Equals(
                                            directory,
                                            StringComparison.OrdinalIgnoreCase
                                        )
                                    ),
                                fallbackDestinationDirectory
                            );

                            if (!Directory.Exists(sourceDirectory))
                            {
                                throw new NotFoundException(
                                    nameof(sourceDirectory),
                                    sourceDirectory
                                );
                            }

                            var destinationDirectory = request.ReplaceWorking
                                ? Path.Combine(
                                    workingDirectoryConfig.Value.Value,
                                    WorkingDirectoryConstants.UnitsDirectory,
                                    unit.SnakeCaseName,
                                    fileType.GetSnakeCaseName()
                                )
                                : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                            try
                            {
                                if (!Directory.Exists(destinationDirectory))
                                    Directory.CreateDirectory(destinationDirectory);

                                var sourceFilePath = Path.Combine(
                                    sourceDirectory,
                                    $"PATCH{assetFile.Hash:X8}.PAC"
                                );

                                var packedFile = await mediator.Send(
                                    new UnpackFhmByPathCommand(
                                        sourceFilePath,
                                        destinationDirectory
                                    ),
                                    cancellationToken
                                );
                                packedFiles.Add(packedFile);
                            }
                            finally
                            {
                                if (!request.ReplaceWorking)
                                    Directory.Delete(destinationDirectory, true);
                            }
                        }
                    }
                }
                else
                {
                    List<PatchFile> patchFiles = [];
                    if (request.PatchFileVersions?.Length > 0)
                    {
                        patchFiles = assetFile
                            .PatchFiles.Where(file =>
                                request.PatchFileVersions.Contains(file.TblId)
                            )
                            .ToList();
                    }
                    else
                    {
                        var latestPatchFile = assetFile
                            .PatchFiles.OrderBy(file => file.TblId)
                            .LastOrDefault();
                        if (latestPatchFile is not null)
                            patchFiles = [latestPatchFile];
                    }

                    foreach (var patchFile in patchFiles)
                    {
                        if (patchFile.AssetFile is null)
                            continue;

                        var tblName = patchFile.TblId.GetPatchName();
                        var fileTypeName = fileType.GetSnakeCaseName();

                        var sourceDirectory = Path.Combine(
                            stagingDirectoryConfig.Value.Value,
                            StagingDirectoryConstants.PsarcDirectory,
                            tblName,
                            StagingDirectoryConstants.CommonDirectory,
                            fileTypeName
                        );

                        if (!Directory.Exists(sourceDirectory))
                            throw new NotFoundException(nameof(sourceDirectory), sourceDirectory);

                        var destinationDirectory = request.ReplaceWorking
                            ? Path.Combine(
                                workingDirectoryConfig.Value.Value,
                                WorkingDirectoryConstants.CommonDirectory,
                                fileTypeName
                            )
                            : Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                        try
                        {
                            if (!Directory.Exists(destinationDirectory))
                                Directory.CreateDirectory(destinationDirectory);

                            var sourceFilePath = Path.Combine(
                                sourceDirectory,
                                $"PATCH{assetFile.Hash:X8}.PAC"
                            );

                            var packedFile = await mediator.Send(
                                new UnpackFhmByPathCommand(sourceFilePath, destinationDirectory),
                                cancellationToken
                            );
                            packedFiles.Add(packedFile);
                        }
                        finally
                        {
                            if (!request.ReplaceWorking)
                                Directory.Delete(destinationDirectory, true);
                        }
                    }
                }
            }
        }

        var tarFileBytes = await compressor.CompressAsync(
            packedFiles,
            CompressionFormats.Tar,
            cancellationToken
        );
        return new FileInfo(tarFileBytes, "unpacked-fhm.tar", MediaTypeNames.Application.Octet);
    }
}
