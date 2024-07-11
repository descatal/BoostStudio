using System.Net.Mime;
using System.Text.Json;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record ExportHitboxGroupCommand(uint[] Hashes) : IRequest<FileInfo>;

public class ExportHitboxGroupCommandHandler(
    IHitboxGroupBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    ICompressor compressor,
    ILogger<ExportHitboxGroupCommandHandler> logger
) : IRequestHandler<ExportHitboxGroupCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(ExportHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await applicationDbContext.HitboxGroups
            .Include(group => group.Unit)
            .Include(group => group.Hitboxes)
            .Where(group => command.Hashes.Contains(group.Hash))
            .ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var hitboxGroup in group)
        {
            var serializedBytes = await binarySerializer.SerializeAsync(hitboxGroup, cancellationToken);
            var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(hitboxGroup.Unit?.Name ?? hitboxGroup.Hash.ToString());
            fileName = Path.ChangeExtension(fileName, ".hitbox");
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        var tarFileBytes = await compressor.CompressAsync(fileInfo, CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "hitbox.tar", MediaTypeNames.Application.Octet);
    }
}
