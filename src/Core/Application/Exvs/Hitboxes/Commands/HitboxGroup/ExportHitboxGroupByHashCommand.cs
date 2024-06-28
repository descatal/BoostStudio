using System.Text.Json;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;
using HitboxGroupEntity=BoostStudio.Domain.Entities.Unit.Hitboxes.HitboxGroup;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record ExportHitboxGroupByHashCommand(uint Hash) : IRequest<FileInfo>;

public record ExportHitboxGroupByUnitIdCommand(uint UnitId) : IRequest<FileInfo>;

public class ExportHitboxGroupByIdCommandHandler(
    IHitboxGroupBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ExportHitboxGroupByIdCommandHandler> logger
) : IRequestHandler<ExportHitboxGroupByHashCommand, FileInfo>, IRequestHandler<ExportHitboxGroupByUnitIdCommand, FileInfo>
{
    public async Task<FileInfo> Handle(ExportHitboxGroupByHashCommand command, CancellationToken cancellationToken)
    {
        var group = await applicationDbContext.HitboxGroups
            .Include(group => group.Unit)
            .Include(group => group.Hitboxes)
            .Where(group => command.Hash == group.Hash)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.Hash, group);
        return await SerializeEntity(group, cancellationToken);
    }

    public async Task<FileInfo> Handle(ExportHitboxGroupByUnitIdCommand command, CancellationToken cancellationToken)
    {
        var group = await applicationDbContext.HitboxGroups
            .Include(group => group.Unit)
            .Include(group => group.Hitboxes)
            .Where(group => command.UnitId == group.GameUnitId)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.UnitId, group);
        return await SerializeEntity(group, cancellationToken);
    }

    private async Task<FileInfo> SerializeEntity(HitboxGroupEntity group, CancellationToken cancellationToken = default)
    {
        var serializedBytes = await binarySerializer.SerializeAsync(group, cancellationToken);
        var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(group.Unit?.Name ?? group.Hash.ToString());
        fileName = Path.ChangeExtension(fileName, ".hitbox");
        return new FileInfo(serializedBytes, fileName);
    }
}
