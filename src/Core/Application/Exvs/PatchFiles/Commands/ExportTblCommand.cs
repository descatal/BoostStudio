using System.Net.Mime;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record ExportTblCommand(PatchFileVersion[]? Versions = null) : IRequest<FileInfo>;

public class ExportTblCommandHandler(
    IApplicationDbContext applicationDbContext,
    ITblBinarySerializer tblBinarySerializer,
    ICompressor compressor
) : IRequestHandler<ExportTblCommand, FileInfo>
{
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
        }
        
        var tarFileBytes = await compressor.CompressAsync(tblBinaries, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "tbl.tar", MediaTypeNames.Application.Octet);
    }
}
