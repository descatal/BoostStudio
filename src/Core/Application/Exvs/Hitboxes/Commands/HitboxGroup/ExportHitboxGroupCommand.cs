using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Common.Utils;
using BoostStudio.Application.Exvs.Fhm.Commands;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reloaded.Memory;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record ExportHitboxGroupCommand(
    uint[]? Hashes = null,
    uint[]? UnitIds = null,
    bool ReplaceWorking = false, // ignored if hot reload is true
    bool HotReload = false // if hot reload is true, it will replace working directory
) : IRequest<FileInfo>;

// TODO: deprecate this
public record ExportHitboxGroupByPathCommand(
    uint[]? Hashes = null,
    uint[]? UnitIds = null,
    string? OutputPath = null
) : IRequest;

public class ExportHitboxGroupCommandHandler(
    IMediator mediator,
    IHitboxGroupBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    ICompressor compressor,
    ILogger<ExportHitboxGroupCommandHandler> logger
)
    : IRequestHandler<ExportHitboxGroupCommand, FileInfo>,
        IRequestHandler<ExportHitboxGroupByPathCommand>
{
    public async ValueTask<FileInfo> Handle(
        ExportHitboxGroupCommand command,
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

        var generatedBinaries = await GenerateBinary(
            command.Hashes,
            command.UnitIds,
            cancellationToken
        );

        var hitboxesWorkingDirectory = Path.Combine(
            workingDirectory.Value.Value,
            "common",
            AssetFileType.Hitboxes.GetSnakeCaseName()
        );
        foreach (var generatedBinary in generatedBinaries)
        {
            if (command is { HotReload: false, ReplaceWorking: false })
                continue;

            var workingFilePath = Path.Combine(hitboxesWorkingDirectory, generatedBinary.FileName);
            await File.WriteAllBytesAsync(workingFilePath, generatedBinary.Data, cancellationToken);
        }

        if (command.HotReload)
        {
            // pack hitboxes in fhm format
            // this implicitly assumes that all units' hitboxes that's required by the game already have a copy in the working directory
            var packedHitboxBinary = await mediator.Send(
                new PackFhmByAssetCommand(
                    AssetFileTypes: [AssetFileType.Hitboxes],
                    ReplaceStaging: false
                ),
                cancellationToken
            );

            // the command will return a tar file, decompress it
            var file = await compressor.DecompressAsync(packedHitboxBinary.Data, cancellationToken);
            var binary = file.FirstOrDefault(); // take the first one lol, we assumin shit here
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
                // 0x40B20000 is the fixed whole packed hitbox offset
                var rpcs3Memory = new ExternalMemory(rpcs3Process);
                rpcs3Memory.WriteRaw((UIntPtr)(mapRegionPointer + 0x40B20000), binary.Data);

#pragma warning restore CA1416
            }
        }

        var tarFileBytes = await compressor.CompressAsync(
            generatedBinaries,
            CompressionFormats.Tar,
            cancellationToken
        );
        return new FileInfo(tarFileBytes, "hitbox.tar", MediaTypeNames.Application.Octet);
    }

    // TODO: deprecate this
    public async ValueTask<Unit> Handle(
        ExportHitboxGroupByPathCommand command,
        CancellationToken cancellationToken
    )
    {
        var generatedBinaries = await GenerateBinary(
            command.Hashes,
            command.UnitIds,
            cancellationToken
        );

        var exportPath = command.OutputPath ?? string.Empty;
        if (string.IsNullOrWhiteSpace(command.OutputPath))
        {
            var configPath = await configsRepository.GetConfig(
                ConfigKeys.WorkingDirectory,
                cancellationToken
            );
            Guard.Against.NotFound(ConfigKeys.WorkingDirectory, configPath.Value);

            exportPath = Path.Combine(configPath.Value.Value, "Hitboxes");
        }

        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);

        foreach (var binary in generatedBinaries)
        {
            var filePath = Path.Combine(exportPath, $"{binary.FileName}");
            await File.WriteAllBytesAsync(filePath, binary.Data, cancellationToken);
        }

        return Unit.Value;
    }

    private async ValueTask<List<FileInfo>> GenerateBinary(
        uint[]? hashes = null,
        uint[]? unitIds = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = applicationDbContext
            .HitboxGroups.Include(group => group.Units)
            .Include(group => group.Hitboxes)
            .AsQueryable();

        if (unitIds is not null)
            query = query.Where(group =>
                group.Units.Any(unit => unitIds.Contains(unit.GameUnitId))
            );

        if (hashes is not null)
            query = query.Where(group => hashes.Contains(group.Hash));

        var group = await query.ToListAsync(cancellationToken);

        var fileInfo = new List<FileInfo>();
        foreach (var hitboxGroup in group)
        {
            var serializedBytes = await binarySerializer.SerializeAsync(
                hitboxGroup,
                cancellationToken
            );

            // hitboxes must have the unit's id in the file name so that the information is not lost
            // hitboxes have loose connection to unit id, the game does not care which unit it is tied to, just the group
            // however for easier import / export flow it is better to have the unit id, and the import operation will rely on the unit id in the file name to associate the HitboxGroup with a unit
            var fileName = hitboxGroup.Units.Count switch
            {
                0 => $"common_{hitboxGroup.Hash}",
                1 =>
                    $"{hitboxGroup.Units.First().SnakeCaseName}-{hitboxGroup.Units.First().GameUnitId}",
                _ =>
                    $"combined_{string.Join('-', hitboxGroup.Units.Select(unit => unit.GameUnitId))}",
            };

            fileName = Path.ChangeExtension(fileName, ".hitbox");
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        return fileInfo;
    }
}
