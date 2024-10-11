using System.Net.Mime;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record ExportTblCommand(
    PatchFileVersion[]? Versions = null,
    bool ReplaceStaging = false
) : IRequest<FileInfo>;

public class ExportTblCommandHandler(
    IConfigsRepository configsRepository,
    IApplicationDbContext applicationDbContext,
    ITblBinarySerializer tblBinarySerializer,
    ICompressor compressor
) : IRequestHandler<ExportTblCommand, FileInfo>
{
    private static readonly Dictionary<PatchFileVersion, string> _patchNameMappings = new()
    {
        [PatchFileVersion.Patch1] = "patch_01_00",
        [PatchFileVersion.Patch2] = "patch_02_00",
        [PatchFileVersion.Patch3] = "patch_03_00",
        [PatchFileVersion.Patch4] = "patch_04_00",
        [PatchFileVersion.Patch5] = "patch_05_00",
        [PatchFileVersion.Patch6] = "patch_06_00",
    };
    
    public async ValueTask<FileInfo> Handle(ExportTblCommand request, CancellationToken cancellationToken)
    {
        var tblQuery = applicationDbContext.Tbl
            .Include(tbl => tbl.PatchFiles)
            .ThenInclude(patchFile => patchFile.FileInfo)
            .Include(tbl => tbl.PatchFiles)
            .ThenInclude(patchFile => patchFile.AssetFile)
            .AsQueryable();
        
        if (request.Versions?.Length > 0)
            tblQuery = tblQuery.Where(tbl => request.Versions.Contains(tbl.Id));
        
        var tbl = await tblQuery.ToListAsync(cancellationToken);
        
        var tblBinaries = new List<FileInfo>();
        foreach (var tblEntry in tbl)
        {
            var tblBinary = await tblBinarySerializer.SerializeAsync(tblEntry, cancellationToken);
            tblBinaries.Add(new FileInfo(tblBinary, tblEntry.Id.ToString(), MediaTypeNames.Application.Octet));

            if (!request.ReplaceStaging)
                continue;
            
            var stagingDirectory = await configsRepository.GetConfig(ConfigKeys.StagingDirectory, cancellationToken);
            if (stagingDirectory.IsError || !_patchNameMappings.TryGetValue(tblEntry.Id, out var tblName))
                continue;

            var stagingFilePath = Path.Combine(stagingDirectory.Value.Value, "psarc", tblName, "PATCH.TBL");
            await File.WriteAllBytesAsync(stagingFilePath, tblBinary, cancellationToken);
        }
        
        var tarFileBytes = await compressor.CompressAsync(tblBinaries, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "tbl.tar", MediaTypeNames.Application.Octet);
    }
}
