using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnitEntity = BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Exvs.Units.Commands;

public record ImportPlayableCharactersCommand(
    Stream File
) : IRequest;

public class ImportPlayableCharactersCommandHandler(
    IListInfoBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ImportPlayableCharactersCommandHandler> logger
) : IRequestHandler<ImportPlayableCharactersCommand>
{
    public async ValueTask<Unit> Handle(ImportPlayableCharactersCommand command, CancellationToken cancellationToken)
    {
        var binaryData = await binarySerializer.DeserializeAsync(command.File, cancellationToken);

        if (binaryData.Body.FirstOrDefault() is not ListInfoBinaryFormat.CharacterInfo)
            throw new Exception("The file supplied is not a valid playable character info file format");

        var allBinaryCharacterInfo = binaryData.Body
            .Select(kaitaiStruct => kaitaiStruct as ListInfoBinaryFormat.CharacterInfo)
            .ToList();

        var allBinaryUnitId = allBinaryCharacterInfo
            .Where(info => info is not null)
            .Select(info => info!.UnitId)
            .ToList();

        var unitEntities = await applicationDbContext.Units
            .Include(unit => unit.PlayableCharacter)
            .Where(entity => allBinaryUnitId.Contains(entity.GameUnitId))
            .ToListAsync(cancellationToken);

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

            // add or update any asset file if supplied / not 0
            // if (playableCharacterEntity.MovieAssetHash != null && playableCharacterEntity.MovieAssetHash != 0)
            // {
            //     var movieAsset = assetEntities.FirstOrDefault(assetFile => assetFile.Hash == unitEntity.PlayableSeries?.MovieAssetHash);
            //     if (movieAsset is null)
            //     {
            //         movieAsset = new AssetFile()
            //         {
            //             Hash = (uint)playableCharacterEntity.MovieAssetHash
            //         };
            //         applicationDbContext.AssetFiles.Add(movieAsset);
            //     }
            //     movieAsset.FileType = AssetFileType.Movie;
            //     unitEntity.PlayableSeries!.MovieAsset = movieAsset;
            // }

            // var seriesMetadata = seriesMetadataOptions.Value.FirstOrDefault(option => playableCharacterEntity.SeriesId == option.Id);
            // if (seriesMetadata is not null)
            //     SeriesMapper.UpdateEntityDetailsIfNull(seriesMetadata, unitEntity);
        }

        return default;
    }


}
