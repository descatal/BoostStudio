using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Series.Commands;

public record ExportSeriesCommand(
    bool ReplaceWorking = false
) : IRequest<FileInfo>;

public class ExportPlayableSeriesCommandHandler(
    IConfigsRepository configsRepository,
    IApplicationDbContext applicationDbContext,
    IListInfoBinarySerializer binarySerializer,
    ILogger<ExportPlayableSeriesCommandHandler> logger
) : IRequestHandler<ExportSeriesCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(ExportSeriesCommand command, CancellationToken cancellationToken)
    {
        var workingDirectory = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
        if (command.ReplaceWorking && (workingDirectory.IsError || string.IsNullOrWhiteSpace(workingDirectory.Value.Value)))
            throw new NotFoundException(ConfigKeys.WorkingDirectory, workingDirectory.FirstError.Description);

        var query = applicationDbContext.Series
            .Include(series => series.PlayableSeries)
            .ThenInclude(playableSeries => playableSeries!.MovieAsset)
            .AsQueryable();

        var playableSeries = await query.ToListAsync(cancellationToken);

        var serializedBytes = await binarySerializer.SerializePlayableSeriesAsync(playableSeries, cancellationToken);

        if (command.ReplaceWorking)
        {
            var workingFilePath = Path.Combine(workingDirectory.Value.Value, "common", AssetFileType.ListInfo.GetSnakeCaseName(), "003.bin");
            await File.WriteAllBytesAsync(workingFilePath, serializedBytes, cancellationToken);
        }

        var fileName = Path.ChangeExtension("playableseries", ".listinfo");
        return new FileInfo(serializedBytes, fileName);
    }
}
