﻿using System.Diagnostics;
using System.Net.Mime;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Extensions;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Exvs.Fhm.Commands;
using BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reloaded.Memory;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.ListInfo.Commands;

public record ExportSeriesListInfoCommand(
    uint[]? UnitIds = null,
    bool ReplaceWorking = false
) : IRequest<FileInfo>;

public class ExportUnitProjectileCommandHandler(
    IUnitProjectileBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    IConfigsRepository configsRepository,
    ICompressor compressor,
    IMediator mediator,
    ILogger<ExportUnitProjectileCommandHandler> logger
) : IRequestHandler<ExportSeriesListInfoCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(ExportSeriesListInfoCommand command, CancellationToken cancellationToken)
    {
        // var workingDirectory = await configsRepository.GetConfig(ConfigKeys.WorkingDirectory, cancellationToken);
        // if (command.ReplaceWorking && (workingDirectory.IsError || string.IsNullOrWhiteSpace(workingDirectory.Value.Value)))
        //     throw new NotFoundException(ConfigKeys.WorkingDirectory, workingDirectory.FirstError.Description);
        //
        // // return combined tar file
        // var generatedBinaries = await GenerateBinary(command.UnitIds, cancellationToken);
        //
        // var projectilesWorkingDirectory = Path.Combine(workingDirectory.Value.Value, "common", AssetFileType.Projectiles.GetSnakeCaseName());
        // if (!Directory.Exists(projectilesWorkingDirectory))
        //     Directory.CreateDirectory(projectilesWorkingDirectory);
        //
        // foreach (var generatedBinary in generatedBinaries)
        // {
        //     if (command is { HotReload: false, ReplaceWorking: false })
        //         continue;
        //
        //     var workingFilePath = Path.Combine(projectilesWorkingDirectory, generatedBinary.FileName);
        //     await File.WriteAllBytesAsync(workingFilePath, generatedBinary.Data, cancellationToken);
        // }
        //
        // if (command.HotReload)
        // {
        //     // pack hitboxes in fhm format
        //     // this implicitly assumes that all units' hitboxes that's required by the game already have a copy in the working directory
        //     var packedProjectileBinary = await mediator.Send(
        //         new PackFhmAssetCommand(AssetFileTypes: [AssetFileType.Projectiles], ReplaceStaging: false),
        //         cancellationToken
        //     );
        //
        //     // the command will return a tar file, decompress it
        //     var file = await compressor.DecompressAsync(packedProjectileBinary.Data, cancellationToken);
        //     var binary = file.FirstOrDefault();
        //     if (binary is not null)
        //     {
        //         const long mapRegionPointer = 0x300000000;
        //         const long projectileBinaryOffset = 0x41670000;
        //         const long projectilePointerListOffset = 0x403B4800; // 403B4F00
        //         using var rpcs3Process = Process.GetProcessesByName("rpcs3").FirstOrDefault();
        //
        //         if (rpcs3Process is null)
        //             throw new NotFoundException("No process with name 'rpcs3' was found", "process");
        //
        //         #pragma warning disable CA1416
        //
        //         // TODO: refactor this to a cross platform thing
        //         // 0x41670000 is the fixed whole packed projectile offset
        //         var rpcs3Memory = new ExternalMemory(rpcs3Process);
        //         rpcs3Memory.WriteRaw((UIntPtr)(mapRegionPointer + projectileBinaryOffset), binary.Data);
        //
        //         // need to rewrite the pointer list too, the game does not care where, but the pointer must point to the correct projectile file start address
        //         // first we nuke the pointer area first, up until 0x403B4F00, which is around 0x700 sized chunk
        //         var zeroChunks = new byte[0x700];
        //         rpcs3Memory.WriteRaw((UIntPtr)(mapRegionPointer + projectilePointerListOffset), zeroChunks);
        //
        //         using var stream = new MemoryStream(binary.Data);
        //         using var reader = new BinaryReader(stream);
        //         // seek to the file count offset at 0x10
        //         stream.Seek(0x10, SeekOrigin.Begin);
        //
        //         var fileCount = reader.ReadUInt32BigEndian();
        //         for (int i = 0; i < fileCount; i++)
        //         {
        //             // calculate the offset in game based on the offset provided by the fhm file
        //             var offset = projectileBinaryOffset + reader.ReadUInt32BigEndian();
        //             var inGameOffset = BitConverter.GetBytes((uint)offset).Reverse().ToArray();
        //
        //             // write the new offset
        //             rpcs3Memory.WriteRaw((UIntPtr)(mapRegionPointer + projectilePointerListOffset + i * 4), inGameOffset);
        //         }
        //
        //         #pragma warning restore CA1416
        //     }
        // }

        var tarFileBytes = await compressor.CompressAsync([], CompressionFormats.Tar, cancellationToken);
        return new FileInfo(tarFileBytes, "projectiles.tar", MediaTypeNames.Application.Octet);
    }

    private async ValueTask<List<FileInfo>> GenerateBinary(uint[]? unitIds = null, CancellationToken cancellationToken = default)
    {
        var query = applicationDbContext.UnitProjectiles
            .Include(unitProjectile => unitProjectile.Unit)
            .Include(unitProjectile => unitProjectile.Projectiles)
            .AsQueryable();

        if (unitIds is not null)
            query = query.Where(unitProjectile => unitIds.Contains(unitProjectile.GameUnitId));
        
        var unitProjectiles = await query.ToListAsync(cancellationToken);
        
        var fileInfo = new List<FileInfo>();
        foreach (var unitProjectile in unitProjectiles)
        {
            if (unitProjectile.Unit is null)
                continue;
            
            var serializedBytes = await binarySerializer.SerializeAsync(unitProjectile, cancellationToken);

            // for projectiles the file name can be ambiguous on which unit it is, since the information is already embedded in the file
            // hence we can just use the unit's snake case name, import will read the data inside the binary to associate the projectile info with the unit
            var fileName = Path.ChangeExtension(unitProjectile.Unit.SnakeCaseName, ".projectile");
            fileInfo.Add(new FileInfo(serializedBytes, fileName));
        }

        return fileInfo;
    }
}
