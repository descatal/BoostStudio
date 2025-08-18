using System.Diagnostics;
using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Utils;
using BoostStudio.Application.Exvs.Fhm.Commands;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reloaded.Memory;
using AmmoEntity = BoostStudio.Domain.Entities.Exvs.Ammo.Ammo;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record ExportAmmoCommand(
    bool ReplaceWorking = false, // ignored if hot reload is true
    bool HotReload = false // if hot reload is true, it will replace working directory
) : IRequest<FileInfo>;

public record ExportAmmoByPathCommand(string? ExportPath = null) : IRequest;

public class ExportAmmoCommandHandler(
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    ICompressor compressor,
    IMediator mediator,
    IFormatBinarySerializer<List<AmmoEntity>> ammoBinarySerializer,
    ILogger<ExportAmmoCommandHandler> logger
) : IRequestHandler<ExportAmmoCommand, FileInfo>, IRequestHandler<ExportAmmoByPathCommand>
{
    public async ValueTask<FileInfo> Handle(
        ExportAmmoCommand command,
        CancellationToken cancellationToken
    )
    {
        var workingDirectory = await configsRepository.GetConfig(
            ConfigKeys.WorkingDirectory,
            cancellationToken
        );
        if (
            command.ReplaceWorking
            && (workingDirectory.IsError || string.IsNullOrWhiteSpace(workingDirectory.Value.Value))
        )
            throw new NotFoundException(
                ConfigKeys.WorkingDirectory,
                workingDirectory.FirstError.Description
            );

        var generatedBinary = await GenerateBinary(cancellationToken);

        var ammoWorkingDirectory = Path.Combine(
            workingDirectory.Value.Value,
            "common",
            AssetFileType.Ammo.GetSnakeCaseName()
        );
        if (command.HotReload || command.ReplaceWorking)
        {
            if (!Directory.Exists(ammoWorkingDirectory))
                Directory.CreateDirectory(ammoWorkingDirectory);

            var workingFilePath = Path.Combine(ammoWorkingDirectory, generatedBinary.FileName);
            await File.WriteAllBytesAsync(workingFilePath, generatedBinary.Data, cancellationToken);
        }

        if (command.HotReload)
        {
            // pack ammo in fhm format
            var packedHitboxBinary = await mediator.Send(
                new PackFhmAssetCommand(
                    AssetFileTypes: [AssetFileType.Ammo],
                    ReplaceStaging: false
                ),
                cancellationToken
            );

            // the command will return a tar file, decompress it
            var file = await compressor.DecompressAsync(packedHitboxBinary.Data, cancellationToken);
            var binary = file.FirstOrDefault();
            if (binary is not null)
            {
                const long mapRegionPointer = 0x300000000;
                using var rpcs3Process = ProcessesUtils.GetRpcs3Process();
                if (rpcs3Process is null)
                {
                    throw new NotFoundException(
                        "No process with name 'rpcs3' was found",
                        "process"
                    );
                }

#pragma warning disable CA1416

                // TODO: refactor this to a cross platform thing
                // 0x40B20000 is the fixed whole packed ammo offset
                var rpcs3Memory = new ExternalMemory(rpcs3Process);
                rpcs3Memory.WriteRaw((UIntPtr)(mapRegionPointer + 0x40C60000), binary.Data);

#pragma warning restore CA1416
            }
        }

        return generatedBinary;
    }

    public async ValueTask<Unit> Handle(
        ExportAmmoByPathCommand command,
        CancellationToken cancellationToken
    )
    {
        var generatedBinary = await GenerateBinary(cancellationToken);

        var exportPath = command.ExportPath ?? string.Empty;
        if (string.IsNullOrWhiteSpace(command.ExportPath))
        {
            var configPath = await configsRepository.GetConfig(
                ConfigKeys.WorkingDirectory,
                cancellationToken
            );
            Guard.Against.NotFound(ConfigKeys.WorkingDirectory, configPath.Value);

            exportPath = Path.Combine(configPath.Value.Value, "Ammo");
        }

        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);
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
