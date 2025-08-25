using System.Text.Json;

namespace BoostStudio.Domain.Enums;

public enum AssetFileType
{
    Unknown,

    // Unit specific types
    Dummy,
    Animations,
    ModelAndTexture,
    Data,
    Effects,
    SoundEffects,
    InGamePilotVoiceLines,
    SpriteFrames,
    WeaponSprites,
    VoiceLinesMetadata,
    PilotVoiceLines,

    // Sprites related (to single unit)
    ArcadeSelectionCostume1Sprite,
    ArcadeSelectionCostume2Sprite,
    ArcadeSelectionCostume3Sprite,
    LoadingLeftCostume1Sprite,
    LoadingLeftCostume2Sprite,
    LoadingLeftCostume3Sprite,
    LoadingRightCostume1Sprite,
    LoadingRightCostume2Sprite,
    LoadingRightCostume3Sprite,
    GenericSelectionCostume1Sprite,
    GenericSelectionCostume2Sprite,
    GenericSelectionCostume3Sprite,
    LoadingTargetUnitSprite,
    LoadingTargetPilotCostume1Sprite,
    LoadingTargetPilotCostume2Sprite,
    LoadingTargetPilotCostume3Sprite,
    InGameSortieAndAwakeningPilotCostume1Sprite,
    InGameSortieAndAwakeningPilotCostume2Sprite,
    InGameSortieAndAwakeningPilotCostume3Sprite,
    ResultSmallUnitSprite,
    FigurineSprite,
    LoadingTargetUnitSmallSprite,
    CatalogStorePilotCostume2Sprite,
    CatalogStorePilotCostume3Sprite,

    // Series specific types
    Movie,

    // Common types
    Hitboxes,
    Projectiles,
    Ammo,
    ListInfo,
    UnitCostInfo,
    SharedFigurineSprites,
    MapSelectSprites,
    ArcadeSelectSmallSprites,
    ArcadeSelectUnitNameSprites,
    CameraConfigs,
    CommonEffects,
    CommonEffectParticles,
    CosmeticInfo,
    TextStrings,
    SeriesLogoSprites,
    SeriesLogoSprites2,
}

public static class AssetFileTypeExtensions
{
    public static bool IsUnitSpecific(this AssetFileType fileType) =>
        fileType switch
        {
            AssetFileType.Dummy => true,
            AssetFileType.Animations => true,
            AssetFileType.ModelAndTexture => true,
            AssetFileType.Data => true,
            AssetFileType.Effects => true,
            AssetFileType.SoundEffects => true,
            AssetFileType.InGamePilotVoiceLines => true,
            AssetFileType.WeaponSprites => true,
            AssetFileType.SpriteFrames => true,
            AssetFileType.VoiceLinesMetadata => true,
            AssetFileType.PilotVoiceLines => true,
            AssetFileType.ArcadeSelectionCostume1Sprite => true,
            AssetFileType.ArcadeSelectionCostume2Sprite => true,
            AssetFileType.ArcadeSelectionCostume3Sprite => true,
            AssetFileType.LoadingLeftCostume1Sprite => true,
            AssetFileType.LoadingLeftCostume2Sprite => true,
            AssetFileType.LoadingLeftCostume3Sprite => true,
            AssetFileType.LoadingRightCostume1Sprite => true,
            AssetFileType.LoadingRightCostume2Sprite => true,
            AssetFileType.LoadingRightCostume3Sprite => true,
            AssetFileType.GenericSelectionCostume1Sprite => true,
            AssetFileType.GenericSelectionCostume2Sprite => true,
            AssetFileType.GenericSelectionCostume3Sprite => true,
            AssetFileType.LoadingTargetUnitSprite => true,
            AssetFileType.LoadingTargetPilotCostume1Sprite => true,
            AssetFileType.LoadingTargetPilotCostume2Sprite => true,
            AssetFileType.LoadingTargetPilotCostume3Sprite => true,
            AssetFileType.InGameSortieAndAwakeningPilotCostume1Sprite => true,
            AssetFileType.InGameSortieAndAwakeningPilotCostume2Sprite => true,
            AssetFileType.InGameSortieAndAwakeningPilotCostume3Sprite => true,
            AssetFileType.ResultSmallUnitSprite => true,
            AssetFileType.FigurineSprite => true,
            AssetFileType.LoadingTargetUnitSmallSprite => true,
            AssetFileType.CatalogStorePilotCostume2Sprite => true,
            AssetFileType.CatalogStorePilotCostume3Sprite => true,
            _ => false,
        };

    public static bool IsCommon(this AssetFileType fileType) =>
        fileType switch
        {
            AssetFileType.Hitboxes => true,
            AssetFileType.Projectiles => true,
            AssetFileType.Ammo => true,
            AssetFileType.ListInfo => true,
            AssetFileType.UnitCostInfo => true,
            AssetFileType.SharedFigurineSprites => true,
            AssetFileType.MapSelectSprites => true,
            AssetFileType.ArcadeSelectSmallSprites => true,
            AssetFileType.ArcadeSelectUnitNameSprites => true,
            AssetFileType.CameraConfigs => true,
            AssetFileType.CommonEffects => true,
            AssetFileType.CommonEffectParticles => true,
            AssetFileType.CosmeticInfo => true,
            AssetFileType.TextStrings => true,
            AssetFileType.SeriesLogoSprites => true,
            AssetFileType.SeriesLogoSprites2 => true,
            _ => false,
        };

    public static string GetSnakeCaseName(this AssetFileType fileType) =>
        JsonNamingPolicy.SnakeCaseLower.ConvertName(fileType.ToString());

    public static string? GetAliases(this AssetFileType fileType) =>
        fileType switch
        {
            AssetFileType.Animations => "OMO",
            AssetFileType.Effects => "EIDX",
            AssetFileType.WeaponSprites => "DNSO",
            _ => null,
        };
}
