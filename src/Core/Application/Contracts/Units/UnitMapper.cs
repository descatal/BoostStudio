using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Units.Characters;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;
using Unit=BoostStudio.Domain.Entities.Exvs.Units.Unit;
using CharacterInfo=BoostStudio.Formats.ListInfoBinaryFormat.CharacterInfo;

namespace BoostStudio.Application.Contracts.Units;

[Mapper]
public static partial class UnitMapper
{
    public static partial List<Unit> UnitDtoToUnit(List<UnitDto> dto);

    [MapProperty(nameof(UnitDto.UnitId), nameof(Unit.GameUnitId))]
    public static partial Unit MapToEntity(UnitDto dto);

    public static partial List<UnitDto> MapToDto(List<Unit> entity);

    [MapProperty(nameof(Unit.GameUnitId), nameof(UnitDto.UnitId))]
    public static partial UnitDto MapToDto(Unit entity);

    public static partial Unit Update(Unit entity);
}

[Mapper]
[UseStaticMapper(typeof(PlayableCharacterMapper))]
public static partial class UnitMapper2
{
    public static partial Unit MapToEntity(UnitDto2 dto);

    [MapperIgnoreSource(nameof(Unit.DomainEvents))]
    [MapperIgnoreSource(nameof(Unit.UnitStats))]
    [MapperIgnoreSource(nameof(Unit.UnitProjectiles))]
    [MapperIgnoreSource(nameof(Unit.HitboxGroup))]
    [MapProperty(nameof(Unit.GameUnitId), nameof(UnitDto2.Id))]
    public static partial UnitDto2 MapToDto(Unit entity);

    public static partial IQueryable<UnitDto2> ProjectToDto(IQueryable<Unit> entity);

    public static void UpdateEntityDetailsIfNull(UnitsMetadataOption source, Unit target)
    {
        if (string.IsNullOrWhiteSpace(target.NameEnglish))
            target.NameEnglish = source.NameEnglish;

        if (string.IsNullOrWhiteSpace(target.NameChinese))
            target.NameChinese = source.NameChinese;

        if (string.IsNullOrWhiteSpace(target.NameJapanese))
            target.NameJapanese = source.NameJapanese;

        if (string.IsNullOrWhiteSpace(target.SlugName))
            target.SlugName = source.SlugName;

        if (string.IsNullOrWhiteSpace(target.StagingDirectoryPath))
            target.StagingDirectoryPath = source.StagingDirectoryPath;

        target.HitboxGroupHash ??= source.HitboxGroupHash;
    }

    public static uint UpsertCharacterAssetFiles(
        IApplicationDbContext applicationDbContext,
        uint order,
        Unit unitEntity,
        List<AssetFile> assetEntities,
        CharacterInfo characterInfo)
    {
        var assetOrder = order;
        var characterAssets = CharacterInfoUtils.GetAssetHashes(characterInfo);

        foreach (var characterAsset in characterAssets)
        {
            // check if unitEntity already has the asset entry assigned to it, if it does use it
            var entity = unitEntity.AssetFiles.FirstOrDefault(assetFile => assetFile.Hash == characterAsset.Item1);

            // 0 means there's no asset assigned, try to remove and move on
            if (characterAsset.Item1 == 0)
            {
                // remove association of the existing entity if found
                if (entity is not null)
                    unitEntity.AssetFiles.Remove(entity);

                continue;
            }

            if (entity is null)
            {
                // further check with existing asset entities and see if there's already an entry in db
                // this happens when the asset list is already imported, but is marked as Unknown due to lack of context
                entity = assetEntities.FirstOrDefault(assetFile => assetFile.Hash == characterAsset.Item1);

                // if the entry does not exist in existing asset db, create one with the Hash
                if (entity is null)
                {
                    entity = new AssetFile()
                    {
                        Hash = characterAsset.Item1,
                        Order = assetOrder++,
                    };

                    applicationDbContext.AssetFiles.Add(entity);
                }

                unitEntity.AssetFiles.Add(entity);
            }

            entity.FileType = characterAsset.Item2;
        }

        return assetOrder;
    }
}

[Mapper]
public static partial class PlayableCharacterMapper
{
    [MapperIgnoreTarget(nameof(PlayableCharacter.Unit))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.Series))]
    [MapperIgnoreSource(nameof(CharacterInfo.ArcadeSelectionCostume1SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.ArcadeSelectionCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.ArcadeSelectionCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingLeftCostume1SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingLeftCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingLeftCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingRightCostume1SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingRightCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingRightCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.GenericSelectionCostume1SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.GenericSelectionCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.GenericSelectionCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingTargetUnitSpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingTargetPilotCostume1SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingTargetPilotCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingTargetPilotCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.InGameSortieAndAwakeningPilotCostume1SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.InGameSortieAndAwakeningPilotCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.InGameSortieAndAwakeningPilotCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.SpriteFramesAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.ResultSmallUnitSpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.FigurineSpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.LoadingTargetUnitSmallSpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.CatalogStorePilotCostume2SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.CatalogStorePilotCostume3SpriteAssetHash))]
    [MapperIgnoreSource(nameof(CharacterInfo.ReleaseString))]
    [MapperIgnoreSource(nameof(CharacterInfo.ReleaseStringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.PStringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.FStringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.FOutStringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.CatalogStorePilotCostume2StringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.CatalogStorePilotCostume3StringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.CatalogStorePilotCostume2TStringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.CatalogStorePilotCostume3TStringOffset))]
    [MapperIgnoreSource(nameof(CharacterInfo.M_Root))]
    [MapperIgnoreSource(nameof(CharacterInfo.M_Io))]
    [MapperIgnoreSource(nameof(CharacterInfo.M_Parent))]
    public static partial PlayableCharacter MapToEntity(CharacterInfo entity);
}

public static class CharacterInfoUtils
{
    public static (uint, AssetFileType)[] GetAssetHashes(CharacterInfo characterInfo) =>
    [
        (characterInfo.ArcadeSelectionCostume1SpriteAssetHash, AssetFileType.ArcadeSelectionCostume1Sprite),
        (characterInfo.ArcadeSelectionCostume2SpriteAssetHash, AssetFileType.ArcadeSelectionCostume2Sprite),
        (characterInfo.ArcadeSelectionCostume3SpriteAssetHash, AssetFileType.ArcadeSelectionCostume3Sprite),
        (characterInfo.LoadingLeftCostume1SpriteAssetHash, AssetFileType.LoadingLeftCostume1Sprite),
        (characterInfo.LoadingLeftCostume2SpriteAssetHash, AssetFileType.LoadingLeftCostume2Sprite),
        (characterInfo.LoadingLeftCostume3SpriteAssetHash, AssetFileType.LoadingLeftCostume3Sprite),
        (characterInfo.LoadingRightCostume1SpriteAssetHash, AssetFileType.LoadingRightCostume1Sprite),
        (characterInfo.LoadingRightCostume2SpriteAssetHash, AssetFileType.LoadingRightCostume2Sprite),
        (characterInfo.LoadingRightCostume3SpriteAssetHash, AssetFileType.LoadingRightCostume3Sprite),
        (characterInfo.GenericSelectionCostume1SpriteAssetHash, AssetFileType.GenericSelectionCostume1Sprite),
        (characterInfo.GenericSelectionCostume2SpriteAssetHash, AssetFileType.GenericSelectionCostume2Sprite),
        (characterInfo.GenericSelectionCostume3SpriteAssetHash, AssetFileType.GenericSelectionCostume3Sprite),
        (characterInfo.LoadingTargetUnitSpriteAssetHash, AssetFileType.LoadingTargetUnitSprite),
        (characterInfo.LoadingTargetPilotCostume1SpriteAssetHash, AssetFileType.LoadingTargetPilotCostume1Sprite),

        // These two maps to the same costume 1 type, because of bandai
        (characterInfo.LoadingTargetPilotCostume2SpriteAssetHash, AssetFileType.LoadingTargetPilotCostume1Sprite),
        (characterInfo.LoadingTargetPilotCostume3SpriteAssetHash, AssetFileType.LoadingTargetPilotCostume1Sprite),

        (characterInfo.InGameSortieAndAwakeningPilotCostume1SpriteAssetHash, AssetFileType.InGameSortieAndAwakeningPilotCostume1Sprite),
        (characterInfo.InGameSortieAndAwakeningPilotCostume2SpriteAssetHash, AssetFileType.InGameSortieAndAwakeningPilotCostume2Sprite),
        (characterInfo.InGameSortieAndAwakeningPilotCostume3SpriteAssetHash, AssetFileType.InGameSortieAndAwakeningPilotCostume3Sprite),
        (characterInfo.SpriteFramesAssetHash, AssetFileType.SpriteFrames),
        (characterInfo.ResultSmallUnitSpriteAssetHash, AssetFileType.ResultSmallUnitSprite),
        (characterInfo.FigurineSpriteAssetHash, AssetFileType.FigurineSprite),
        (characterInfo.LoadingTargetUnitSmallSpriteAssetHash, AssetFileType.LoadingTargetUnitSmallSprite),
        (characterInfo.CatalogStorePilotCostume2SpriteAssetHash, AssetFileType.CatalogStorePilotCostume2Sprite),
        (characterInfo.CatalogStorePilotCostume3SpriteAssetHash, AssetFileType.CatalogStorePilotCostume3Sprite),
    ];
}
