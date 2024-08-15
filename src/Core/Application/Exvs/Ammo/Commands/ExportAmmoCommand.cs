using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AmmoEntity=BoostStudio.Domain.Entities.Unit.Ammo.Ammo;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record ExportAmmoCommand : IRequest<FileInfo>;
public record ExportAmmoByPathCommand(string? ExportPath = null) : IRequest;

public class ExportAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    IFormatBinarySerializer<List<AmmoEntity>> ammoBinarySerializer,
    ILogger<ExportAmmoCommandHandler> logger
) : IRequestHandler<ExportAmmoCommand, FileInfo>,
    IRequestHandler<ExportAmmoByPathCommand>
{
    public async ValueTask<FileInfo> Handle(ExportAmmoCommand command, CancellationToken cancellationToken)
        => await GenerateBinary(cancellationToken);

    public async ValueTask<Unit> Handle(ExportAmmoByPathCommand command, CancellationToken cancellationToken)
    {
        var generatedBinary = await GenerateBinary(cancellationToken);
        
        var exportPath = command.ExportPath ?? string.Empty;
        if (string.IsNullOrWhiteSpace(command.ExportPath))
        {
            var configPath = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
            Guard.Against.NotFound(ConfigKeys.WorkingDirectory, configPath.Value);

            exportPath = Path.Combine(configPath.Value.Value, "Ammo");
        }
        
        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);
        
        var filePath = Path.Combine(exportPath, generatedBinary.FileName);
        await File.WriteAllBytesAsync(filePath, generatedBinary.Data, cancellationToken);

        return Unit.Value;
    }

    private async ValueTask<FileInfo> GenerateBinary(CancellationToken cancellationToken = default)
    {
        var ammo = await applicationDbContext.Ammo.ToListAsync(cancellationToken);
        var serializedBinary = await ammoBinarySerializer.SerializeAsync(ammo, cancellationToken);
        return new FileInfo(serializedBinary, "all.ammo", MediaTypeNames.Application.Octet);
    }
}
