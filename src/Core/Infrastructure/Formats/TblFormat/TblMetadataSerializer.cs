﻿using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Contracts.Metadata.Models;
using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.TblFormat;

public class TblMetadataSerializer : ITblMetadataSerializer
{
    public Task<List<PatchFile>> SerializeAsync(
        Tbl tbl,
        TblBinaryFormat data,
        CancellationToken cancellationToken = default)
    {
        var patchFiles = new List<PatchFile>();
        var existingPatchFiles = tbl.PatchFiles.ToList();
        
        for (var index = 0; index < data.CumulativeFileCount; index++)
        {
            var fileInfoBody = data.FileInfos[index];
            if (fileInfoBody?.FileInfo is null)
                continue;

            var patchFile = existingPatchFiles.FirstOrDefault(patchFile => 
                patchFile.FileInfo is not null && 
                patchFile.AssetFileHash is not null &&
                patchFile.AssetFileHash == fileInfoBody.FileInfo.HashName
            );
            
            if (patchFile is null)
            {
                patchFile = new PatchFile();
                patchFiles.Add(patchFile);
            }

            // if the asset file is not in the database, add it
            if (patchFile.AssetFileHash is null || patchFile.AssetFile is null)
                patchFile.AssetFile = new AssetFile();
            
            patchFile.AssetFile.Hash = fileInfoBody.FileInfo.HashName;
            patchFile.AssetFile.Order = (uint)index;
            
            var patchFileInfo = patchFile.FileInfo ?? new PatchFileInfo();
            
            patchFileInfo.Version = (PatchFileVersion)fileInfoBody.FileInfo.PatchNumber;
            patchFileInfo.Size1 = fileInfoBody.FileInfo.Size1;
            patchFileInfo.Size2 = fileInfoBody.FileInfo.Size2;
            patchFileInfo.Size3 = fileInfoBody.FileInfo.Size3;

            PathInfo? pathInfo = null;
            if (fileInfoBody.FileInfo.PathBody?.Path is not null)
            {
                pathInfo = patchFile.PathInfo ?? new PathInfo();
                pathInfo.Path = fileInfoBody.FileInfo.PathBody.Path;
                pathInfo.Order = (uint)fileInfoBody.FileInfo.PathBody.Index;
            }

            patchFile.PathInfo = pathInfo;
            patchFile.FileInfo = patchFileInfo;
            patchFiles.Add(patchFile);
        }

        var fileInfoPathIndices = data.FileInfos
            .OrderBy(fileInfoBody => fileInfoBody.Offset)
            .Select(fileInfoBody => fileInfoBody?.FileInfo?.PathIndex)
            .Where(index => index is not null)
            .ToList();

        var filePathsWithoutInfo = data.FilePaths
            .Where(((_, index) => !fileInfoPathIndices.Contains(index)))
            .Select(filePathBody => new PatchFile()
            {
                PathInfo = new PathInfo
                {
                    Path = filePathBody.Path,
                    Order = (uint)filePathBody.Index
                }
            })
            .ToList();

        patchFiles.AddRange(filePathsWithoutInfo);

        return Task.FromResult(patchFiles);
    }
    
    public Task<TblDto> SerializeDtoAsync(TblBinaryFormat data, CancellationToken cancellationToken)
    {
        var fileMetadata = new List<PatchFileMetadataDto>();
    
        for (var index = 0; index < data.CumulativeFileCount; index++)
        {
            var fileInfoBody = data.FileInfos[index];
            var fileInfo = fileInfoBody.FileInfo;
            if (fileInfoBody.FileInfo is null || fileInfo is null)
                continue;
    
            // Parse the file info entries normally. 
            var infoMetadata = new TblFileInfoMetadata(
                CumulativeIndex: (uint)fileInfoBody.Index,
                PatchNumber: fileInfo.PatchNumber,
                Size1: fileInfo.Size1,
                Size2: fileInfo.Size2,
                Size3: fileInfo.Size3,
                Size4: fileInfo.Size4,
                HashName: fileInfo.HashName);
    
            fileMetadata.Add(new PatchFileMetadataDto(FileInfoMetadata: infoMetadata, Path: fileInfo.PathBody?.Path));
        }
    
        var fileInfoPathIndices = data.FileInfos
            .OrderBy(fileInfoBody => fileInfoBody.Offset)
            .Select(fileInfoBody => fileInfoBody?.FileInfo?.PathIndex)
            .Where(index => index is not null)
            .ToList();
    
        var filePathsWithoutInfo = data.FilePaths
            .Where(((_, i) => !fileInfoPathIndices.Contains(i)))
            .Select(filePathBody => new PatchFileMetadataDto(Path: filePathBody?.Path))
            .ToList();
    
        fileMetadata.AddRange(filePathsWithoutInfo);
    
        // If there's any paths without infos, or if the order of the file info path index are not in ascending order.
        // Add a file path order list to the json file so it can be used to determine the order of the path list originally in binary.
        var filePathOrder = (filePathsWithoutInfo.Count > 0 || !fileInfoPathIndices.SequenceEqual(fileInfoPathIndices.OrderBy(index => index)))
            ? data.FilePaths.Select(body => body.Path).ToList()
            : null;
    
        var tblMetadata = new TblDto(
            CumulativeFileInfoCount: data.CumulativeFileCount,
            FileMetadata: fileMetadata,
            PathOrder: filePathOrder);
    
        return Task.FromResult(tblMetadata);
    }

    public Task<TblBinaryFormat> DeserializeDtoAsync(Stream stream, TblDto data, CancellationToken cancellationToken)
    {
        // Use the passed in stream so that the stream and their child can be disposed later by the caller
        var tbl = new TblBinaryFormat(
            p_totalFileSize: 0, // Not sure if setting to 0 will cause issue down the line or not...
            p__io: new KaitaiStream(stream),
            write: true)
            {
                CumulativeFileCount = data.CumulativeFileInfoCount, FileInfos = [], FilePaths = [],
            };

        var paths = data.FileMetadata
            .Select(meta => meta.Path)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(path => path!)
            .ToList();

        var sortedPaths = data.PathOrder is not null
            ? paths
                .Where(item => data.PathOrder.Contains(item))
                .OrderBy(item => data.PathOrder.IndexOf(item))
                .ToList()
            : paths;

        var filePaths = new Dictionary<int, TblBinaryFormat.FilePathBody>();
        foreach ((string? filePath, TblFileInfoMetadata? fileInfoMetadata) in data.FileMetadata)
        {
            var filePathIndex = 0;
            TblBinaryFormat.FilePathBody? filePathBody = null;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                filePathIndex = sortedPaths.IndexOf(filePath);
                filePathBody = new TblBinaryFormat.FilePathBody(
                    p_index: filePathIndex, p__io: new KaitaiStream(new MemoryStream()), p__parent: tbl, p__root: tbl, write: true)
                    {
                        Path = filePath
                    };
                filePaths.Add(filePathIndex, filePathBody);
            }

            if (fileInfoMetadata is null)
                continue;

            var fileInfoBody = new TblBinaryFormat.FileInfoBody(
                p_index: (int)fileInfoMetadata.CumulativeIndex, p__io: new KaitaiStream(stream), p__parent: tbl, p__root: tbl, write: true);

            var fileInfo = new TblBinaryFormat.FileInfo(
                p__io: new KaitaiStream(new MemoryStream()), p__parent: fileInfoBody, p__root: tbl, write: true)
                {
                    PatchNumber = fileInfoMetadata.PatchNumber,
                    PathIndex = filePathIndex,
                    Unk8 = new byte[]
                    {
                        0x0, 0x4, 0x0, 0x0
                    },
                    Size1 = fileInfoMetadata.Size1,
                    Size2 = fileInfoMetadata.Size2,
                    Size3 = fileInfoMetadata.Size3,
                    Size4 = fileInfoMetadata.Size4,
                    HashName = fileInfoMetadata.HashName,
                    PathBody = filePathBody
                };

            fileInfoBody.FileInfo = fileInfo;
            tbl.FileInfos.Add(fileInfoBody);
        }

        tbl.FilePaths = filePaths
            .OrderBy(pair => pair.Key)
            .Select(pair => pair.Value)
            .ToList();

        return Task.FromResult(tbl);
    }
}
