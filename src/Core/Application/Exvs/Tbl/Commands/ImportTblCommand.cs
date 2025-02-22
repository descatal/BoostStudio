﻿using BoostStudio.Application.Common.Enums.Assets;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Tbl.Commands;

public record ImportTblCommand(Stream[] Files) : IRequest;

public class ImportTblCommandHandler(
    ITblBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext
) : IRequestHandler<ImportTblCommand>
{
    public async ValueTask<Unit> Handle(ImportTblCommand command, CancellationToken cancellationToken)
    {
        var deserializedTblBinaryData = new Dictionary<PatchFileVersion, TblBinaryFormat>();
        foreach (var fileStream in command.Files)
        {
            var binaryData = await binarySerializer.DeserializeAsync(fileStream, cancellationToken);

            // to determine what's the tbl version, select the highest version of the file info
            var version = binaryData.FileInfos
                .Where(body => body.FileInfo is not null)
                .Max(body => body.FileInfo.PatchNumber);

            deserializedTblBinaryData.Add((PatchFileVersion)version, binaryData);
        }

        var existingTbl = await applicationDbContext.Tbl
            .Include(tbl => tbl.PatchFiles)
            .ThenInclude(patchFile => patchFile.FileInfo)
            .Include(tbl => tbl.PatchFiles)
            .ThenInclude(patchFile => patchFile.AssetFile)
            .Where(tbl => deserializedTblBinaryData.Keys.Contains(tbl.Id))
            .ToListAsync(cancellationToken);

        // load all existing asset file that's associated with this binary
        var assetFileHashes = deserializedTblBinaryData.Values
            .SelectMany(binaryFormat => binaryFormat.FileInfos)
            .Where(fileInfo => fileInfo?.FileInfo is not null)
            .Select(fileInfo => fileInfo.FileInfo.HashName)
            .ToList();

        var existingAssetFiles = applicationDbContext.AssetFiles
            .Where(assetFile => assetFileHashes.Contains(assetFile.Hash))
            .ToList();

        var exvsCommonAssets = Enum.GetValues<ExvsCommonAssets>();
        foreach ((PatchFileVersion version, TblBinaryFormat binaryData) in deserializedTblBinaryData)
        {
            var tbl = existingTbl.FirstOrDefault(entity => entity.Id == version);
            if (tbl is null)
            {
                tbl = new Domain.Entities.Exvs.Tbl.Tbl
                {
                    Id = version
                };
                applicationDbContext.Tbl.Add(tbl);
            }

            var patchFiles = new List<PatchFile>();
            var existingTblPatchFiles = tbl.PatchFiles.ToList();
            for (var index = 0; index < binaryData.CumulativeFileCount; index++)
            {
                var fileInfoBody = binaryData.FileInfos[index];
                if (fileInfoBody?.FileInfo is null)
                    continue;

                // patch file matching (fuck you bandai):
                // 1. both path and the asset file hash matches OR
                // 2. only asset file hash match OR
                // 3. only path match

                // both path and the asset file hash matches
                var patchFile = existingTblPatchFiles.FirstOrDefault(file =>
                    file.FileInfo is not null &&
                    file.AssetFileHash == fileInfoBody.FileInfo.HashName);

                // if everything fails create a new PatchFile entry
                if (patchFile is null)
                {
                    patchFile = new PatchFile();
                    existingTblPatchFiles.Add(patchFile);
                }

                var assetFile = existingAssetFiles.FirstOrDefault(assetFile => assetFile.Hash == fileInfoBody.FileInfo.HashName);
                if (assetFile is null)
                {
                    assetFile = new AssetFile();
                    applicationDbContext.AssetFiles.Add(assetFile);
                }

                assetFile.Order = (uint)(index + 1); // db can't store 0 because of ValueGeneratedOnAdd
                assetFile.Hash = fileInfoBody.FileInfo.HashName;

                var commonAsset = exvsCommonAssets.FirstOrDefault(commonAssets => (uint)commonAssets == assetFile.Hash, ExvsCommonAssets.Unknown);
                if (commonAsset != ExvsCommonAssets.Unknown)
                    assetFile.AddFileType(commonAsset.GetAssetFileType());

                patchFile.AssetFile = assetFile;

                var patchFileInfo = patchFile.FileInfo ?? new PatchFileInfo();

                patchFileInfo.Version = (PatchFileVersion)fileInfoBody.FileInfo.PatchNumber;
                patchFileInfo.Size1 = fileInfoBody.FileInfo.Size1;
                patchFileInfo.Size2 = fileInfoBody.FileInfo.Size2;
                patchFileInfo.Size3 = fileInfoBody.FileInfo.Size3;
                patchFileInfo.Size4 = fileInfoBody.FileInfo.Size4;

                // path info can be null
                PathInfo? pathInfo = null;
                if (!string.IsNullOrWhiteSpace(fileInfoBody.FileInfo.PathBody?.Path))
                {
                    pathInfo = patchFile.PathInfo ?? new PathInfo();
                    pathInfo.Path = fileInfoBody.FileInfo.PathBody.Path;
                    pathInfo.Order = (uint)fileInfoBody.FileInfo.PathBody.Index;
                }

                patchFile.PathInfo = pathInfo;
                patchFile.FileInfo = patchFileInfo;

                patchFiles.Add(patchFile);
            }

            var fileInfoPathIndices = binaryData.FileInfos
                .OrderBy(fileInfoBody => fileInfoBody.Offset)
                .Select(fileInfoBody => fileInfoBody?.FileInfo?.PathIndex)
                .Where(index => index is not null)
                .ToList();

            var filePathsWithoutInfo = binaryData.FilePaths
                .Where(((_, index) => !fileInfoPathIndices.Contains(index)))
                .ToList();

            foreach (var filePathBody in filePathsWithoutInfo)
            {
                var patchFile = existingTblPatchFiles.FirstOrDefault(patchFile =>
                    patchFile.PathInfo is not null &&
                    !string.IsNullOrWhiteSpace(patchFile.PathInfo.Path) &&
                    patchFile.PathInfo.Path.Equals(filePathBody.Path, StringComparison.OrdinalIgnoreCase));

                if (patchFile is null)
                {
                    patchFile = new PatchFile();
                    existingTblPatchFiles.Add(patchFile);
                }

                var pathInfo = patchFile.PathInfo ?? new PathInfo();
                pathInfo.Path = filePathBody.Path;
                pathInfo.Order = (uint)filePathBody.Index;

                patchFile.PathInfo = pathInfo;
                patchFile.FileInfo = null;
                patchFile.AssetFileHash = null;
                patchFile.AssetFile = null;

                patchFiles.Add(patchFile);
            }

            tbl.CumulativeAssetIndex = binaryData.CumulativeFileCount;
            tbl.PatchFiles = patchFiles;
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;

        // await using var fileStream = new MemoryStream(request.File);
        // var tbl = await binarySerializer.DeserializeAsync(fileStream, request.UseSubfolderFlag, cancellationToken);
        // var asd = await tblMetadataSerializer.SerializeAsync(tbl, cancellationToken);
    }
}
