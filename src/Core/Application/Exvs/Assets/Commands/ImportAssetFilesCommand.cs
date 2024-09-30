using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Unit.Assets;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Assets.Commands;

public record ImportAssetFilesCommand(Stream[] Files) : IRequest;

public class ImportAssetFileCommandHandler(
    IAssetFilesBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext
) : IRequestHandler<ImportAssetFilesCommand>
{
    public async ValueTask<Unit> Handle(ImportAssetFilesCommand command, CancellationToken cancellationToken)
    {
        var binaries = new List<AssetFilesBinaryFormat>();
        foreach (var fileStream in command.Files)
        {
            var binaryData = await binarySerializer.DeserializeAsync(fileStream, cancellationToken);
            binaries.Add(binaryData);
        }

        foreach (var binaryData in binaries)
        {
            var unitFiles = binaryData.Files.ToDictionary(
                file => file.UnitId,
                hashes => new Dictionary<AssetFileType, uint>
                {
                    [AssetFileType.Dummy] = hashes.DummyHash,
                    [AssetFileType.Data] = hashes.DataHash,
                    [AssetFileType.Models] = hashes.ModelHash,
                    [AssetFileType.Animations] = hashes.AnimationHash,
                    [AssetFileType.Effects] = hashes.EffectsHash,
                    [AssetFileType.SoundEffects] = hashes.SoundEffectsHash,
                    [AssetFileType.InGamePilotVoiceLines] = hashes.InGamePilotVoiceLinesHash,
                    [AssetFileType.WeaponSprites] = hashes.WeaponSpritesHash,
                    [AssetFileType.InGameCutInSprites] = hashes.InGameCutInSpritesHash,
                    [AssetFileType.SpriteFrames] = hashes.SpriteFramesHash,
                    [AssetFileType.VoiceLinesMetadata] = hashes.VoiceLinesMetadataHash,
                    [AssetFileType.PilotVoiceLines] = hashes.PilotVoiceLinesHash,
                });

            var unitIds = unitFiles.Keys.ToList();
            var fileHashes = unitFiles.Values
                .SelectMany(map => map.Values)
                .ToList();

            // import can be two ways, either assign an existing file to a unit, or create a new one
            var existingAssets = await applicationDbContext.AssetFiles
                .Include(asset => asset.Units)
                .Where(asset => 
                    (asset.Units.Any(unit => unitIds.Contains(unit.GameUnitId))) || 
                    fileHashes.Contains(asset.Hash)
                )
                .ToListAsync(cancellationToken);

            var existingUnits = await applicationDbContext.Units
                .Where(unit => unitIds.Contains(unit.GameUnitId))
                .ToListAsync(cancellationToken);

            foreach ((uint unitId, Dictionary<AssetFileType, uint>? files) in unitFiles)
            {
                var unitEntity = existingUnits.FirstOrDefault(unit => unit.GameUnitId == unitId);
                if (unitEntity is null)
                    continue;
                
                var hashes = files.Values.Select(u => u).ToList();
                var existingRelatedAssets = existingAssets
                    .Where(asset => 
                        asset.Units.Any(unit => unit.GameUnitId == unitId) || 
                        hashes.Contains(asset.Hash)
                    )
                    .ToList();
                
                foreach ((AssetFileType fileType, uint hash) in files)
                {
                    if (hash == 0xFFFFFFFF)
                        continue;
                    
                    var assetFile = existingRelatedAssets.FirstOrDefault(asset => asset.Hash == hash);
                    
                    if (assetFile is null)
                    {
                        assetFile = new AssetFile
                        {
                            Order = 1, // since we don't know the order, defaults to 1, db does not take 0 for some reason
                            Hash = hash,
                        };
                        existingAssets.Add(assetFile);
                        applicationDbContext.AssetFiles.Add(assetFile);
                    }
                    
                    if (assetFile.Units.All(unit => unit.GameUnitId != unitId))
                        assetFile.Units.Add(unitEntity);
                    
                    assetFile.FileType = fileType;
                }
            }

            await applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
