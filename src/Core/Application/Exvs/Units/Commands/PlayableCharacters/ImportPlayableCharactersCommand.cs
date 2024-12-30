using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnitEntity = BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Exvs.Units.Commands.PlayableCharacters;

public record ImportPlayableCharactersCommand(
    Stream File
) : IRequest;

public class ImportPlayableCharactersCommandHandler(
    IListInfoBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    IOptions<List<UnitsMetadataOption>> unitsMetadataOptions,
    ILogger<ImportPlayableCharactersCommandHandler> logger
) : IRequestHandler<ImportPlayableCharactersCommand>
{
    public async ValueTask<Unit> Handle(ImportPlayableCharactersCommand command, CancellationToken cancellationToken)
    {
        var binaryData = await binarySerializer.DeserializeAsync(command.File, cancellationToken);

        if (binaryData.Body.FirstOrDefault() is not ListInfoBinaryFormat.CharacterInfo)
            throw new Exception("The file supplied is not a valid playable character info file format");

        var foo = binaryData.Body
            .Select(x => ((x as ListInfoBinaryFormat.CharacterInfo)!).UnitId)
            .ToList();

        var allBinaryCharacterInfo = binaryData.Body
            .Select(kaitaiStruct => kaitaiStruct as ListInfoBinaryFormat.CharacterInfo)
            .ToList();

        var allBinaryUnitId = allBinaryCharacterInfo
            .Where(info => info is not null)
            .Select(info => info!.UnitId)
            .ToList();

        var allBinaryAssetFileHashes = allBinaryCharacterInfo
            .Where(info => info is not null)
            .SelectMany(info => CharacterAssetUtils.GetAssetHashes(info!).Select(tuple => tuple.Item1))
            .ToList();

        var unitEntities = await applicationDbContext.Units
            .Include(unit => unit.PlayableCharacter)
            .Include(unit => unit.AssetFiles)
            .Where(entity => allBinaryUnitId.Contains(entity.GameUnitId))
            .ToListAsync(cancellationToken);

        var assetEntities = await applicationDbContext.AssetFiles
            .Where(assetFile => allBinaryAssetFileHashes.Contains(assetFile.Hash))
            .ToListAsync(cancellationToken);

        var lastAssetFileOrder = await applicationDbContext.AssetFiles
            .MaxAsync(assetFile => assetFile.Order, cancellationToken: cancellationToken) + 1;

        foreach (var binaryCharacterInfo in allBinaryCharacterInfo)
        {
            if (binaryCharacterInfo is null)
                continue;

            var playableCharacterEntity = PlayableCharacterMapper.MapToEntity(binaryCharacterInfo);
            var unitEntity = unitEntities.FirstOrDefault(unit => unit.GameUnitId == binaryCharacterInfo.UnitId);

            if (unitEntity is null)
            {
                unitEntity = new UnitEntity
                {
                    GameUnitId = binaryCharacterInfo.UnitId
                };
                await applicationDbContext.Units.AddAsync(unitEntity, cancellationToken);
            }
            unitEntity.PlayableCharacter = playableCharacterEntity;

            var characterAssets = CharacterAssetUtils.GetAssetHashes(binaryCharacterInfo);
            lastAssetFileOrder = UnitMapper2.UpsertCharacterAssetFiles(applicationDbContext, lastAssetFileOrder, unitEntity, assetEntities, characterAssets);

            var seriesMetadata = unitsMetadataOptions.Value.FirstOrDefault(option => playableCharacterEntity.UnitId == option.Id);
            if (seriesMetadata is not null)
                UnitMapper2.UpdateEntityDetailsIfNull(seriesMetadata, unitEntity);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}
