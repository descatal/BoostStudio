using BoostStudio.Domain.Entities.Exvs.Units.Characters;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;
using Unit = BoostStudio.Domain.Entities.Exvs.Units.Unit;

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
}

[Mapper]
public static partial class PlayableCharacterMapper
{
    [MapperIgnoreTarget(nameof(PlayableCharacter.Unit))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.ArcadeSelectionCostume1SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.ArcadeSelectionCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.ArcadeSelectionCostume3SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingLeftCostume1SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingLeftCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingLeftCostume3SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingRightCostume1SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingRightCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingRightCostume3SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.GenericSelectionCostume1SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.GenericSelectionCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.GenericSelectionCostume3SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingTargetUnitSpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingTargetPilotCostume1SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingTargetPilotCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingTargetPilotCostume3SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.InGameSortieAndAwakeningPilotCostume1SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.InGameSortieAndAwakeningPilotCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.InGameSortieAndAwakeningPilotCostume3SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.SpriteFramesAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.ResultSmallUnitSprite))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.FigurineSpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.LoadingTargetUnitSmallSpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.CatalogStorePilotCostume2SpriteAsset))]
    [MapperIgnoreTarget(nameof(PlayableCharacter.CatalogStorePilotCostume3SpriteAsset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.ReleaseString))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.ReleaseStringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.PStringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.FStringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.FOutStringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.CatalogStorePilotCostume2StringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.CatalogStorePilotCostume3StringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.CatalogStorePilotCostume2TStringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.CatalogStorePilotCostume3TStringOffset))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.M_Root))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.M_Io))]
    [MapperIgnoreSource(nameof(ListInfoBinaryFormat.CharacterInfo.M_Parent))]
    public static partial PlayableCharacter MapToEntity(ListInfoBinaryFormat.CharacterInfo entity);
}
