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
    WeaponSprites,
    InGameCutInSprites,
    SpriteFrames,
    VoiceLinesMetadata,
    PilotVoiceLines,

    // Common types
    Hitboxes,
    Projectiles,
    Ammo,
    ListInfo,
    UnitCostInfo,
    FigurineSprites,
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
    public static bool IsUnitSpecific(this AssetFileType fileType)
        => fileType switch
        {
            AssetFileType.Dummy => true,
            AssetFileType.Animations => true,
            AssetFileType.ModelAndTexture => true,
            AssetFileType.Data => true,
            AssetFileType.Effects => true,
            AssetFileType.SoundEffects => true,
            AssetFileType.InGamePilotVoiceLines => true,
            AssetFileType.WeaponSprites => true,
            AssetFileType.InGameCutInSprites => true,
            AssetFileType.SpriteFrames => true,
            AssetFileType.VoiceLinesMetadata => true,
            AssetFileType.PilotVoiceLines => true,
            _ => false
        };
    
    public static bool IsCommon(this AssetFileType fileType)
        => fileType switch
        {
            AssetFileType.Hitboxes => true,
            AssetFileType.Projectiles => true,
            AssetFileType.Ammo => true,
            AssetFileType.ListInfo => true,
            AssetFileType.UnitCostInfo => true,
            AssetFileType.FigurineSprites => true,
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
            _ => false
        };

    public static string GetSnakeCaseName(this AssetFileType fileType)
        => JsonNamingPolicy.SnakeCaseLower.ConvertName(fileType.ToString());
}
