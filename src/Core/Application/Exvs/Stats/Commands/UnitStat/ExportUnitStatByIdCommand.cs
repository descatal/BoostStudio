using System.Text.Json;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Stats.Commands.UnitStat;

public record ExportUnitStatByIdCommand(uint UnitId) : IRequest<FileInfo>;

public class ExportUnitStatByIdCommandHandler(
    IUnitStatBinarySerializer statBinarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ExportUnitStatByIdCommandHandler> logger
) : IRequestHandler<ExportUnitStatByIdCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(ExportUnitStatByIdCommand command, CancellationToken cancellationToken)
    {
        var unitStat = await applicationDbContext.UnitStats
            .Include(stat => stat.AmmoSlots)
            .Include(stat => stat.Ammo)
            .Include(stat => stat.Stats)
            .Include(stat => stat.Unit)
            .Where(stat => command.UnitId == stat.GameUnitId)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.UnitId, unitStat);
        
        var serializedBytes = await statBinarySerializer.SerializeAsync(unitStat, cancellationToken);
        var fileName = JsonNamingPolicy.SnakeCaseLower.ConvertName(unitStat.Unit?.Name ?? unitStat.GameUnitId.ToString());
        fileName = Path.ChangeExtension(fileName, ".stats");
        return new FileInfo(serializedBytes, fileName);
    }
}
