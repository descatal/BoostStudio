using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Common.Enums.Assets;

public enum ExvsCommonAssets : uint
{
    Unknown = 0,
    ExvsHitboxes = 0xDF3B4191,
    ExvsProjectiles = 0xAEB4F916,
    ExvsAmmo = 0x3DD6DC78,
    ExvsRosterInfo = 0xEB3A9691,
    ExvsUnitCostInfo = 0x3BEC3AA5,
    ExvsFigurineSprites = 0xDC38B066,
    ExvsMapSelectSprites = 0xA2A919E5,
    ExvsArcadeSelectSmallSprites = 0x511A264D,
    ExvsArcadeSelectUnitNameSprites = 0x6AAD7190,
    ExvsCameraConfigs = 0xBD5D12ED,
    ExvsCommonEffects = 0xFABAA98C,
    ExvsCommonEffectParticles = 0x34F85A51,
    ExvsCosmeticInfo = 0xCCABBB3B,
    ExvsTextStrings = 0x2C7FA9A9,
    ExvsSeriesLogoSprites = 0x35E01A85,
    ExvsSeriesLogoSprites2 = 0x4207FFFA,
}

public static class ExvsCommonAssetsExtensions
{
    // gets the default asset file order index for these common assets
    // the order is based on EXVSFB base game tbl file (should be the same across all Exvs)
    public static uint GetDefaultOrderIndex(this ExvsCommonAssets commonAsset)
    {
        return commonAsset switch
        {
            ExvsCommonAssets.ExvsHitboxes => 2119,
            ExvsCommonAssets.ExvsProjectiles => 2121,
            ExvsCommonAssets.ExvsAmmo => 2120,
            ExvsCommonAssets.ExvsRosterInfo => 3503,
            ExvsCommonAssets.ExvsUnitCostInfo => 2126,
            ExvsCommonAssets.ExvsFigurineSprites => 4834,
            ExvsCommonAssets.ExvsMapSelectSprites => 3251,
            ExvsCommonAssets.ExvsArcadeSelectSmallSprites => 3136,
            ExvsCommonAssets.ExvsArcadeSelectUnitNameSprites => 3135,
            ExvsCommonAssets.ExvsCameraConfigs => 2124,
            ExvsCommonAssets.ExvsCommonEffects => 12,
            ExvsCommonAssets.ExvsCommonEffectParticles => 2122,
            ExvsCommonAssets.ExvsCosmeticInfo => 2123,
            ExvsCommonAssets.ExvsTextStrings => 2113,
            ExvsCommonAssets.ExvsSeriesLogoSprites => 3137,
            ExvsCommonAssets.ExvsSeriesLogoSprites2 => 3138,
            _ => 0,
        };
    }

    public static AssetFileType GetAssetFileType(this ExvsCommonAssets commonAsset)
    {
        return commonAsset switch
        {
            ExvsCommonAssets.ExvsHitboxes => AssetFileType.Hitboxes,
            ExvsCommonAssets.ExvsProjectiles => AssetFileType.Projectiles,
            ExvsCommonAssets.ExvsAmmo => AssetFileType.Ammo,
            ExvsCommonAssets.ExvsRosterInfo => AssetFileType.RosterInfo,
            ExvsCommonAssets.ExvsUnitCostInfo => AssetFileType.UnitCostInfo,
            ExvsCommonAssets.ExvsFigurineSprites => AssetFileType.FigurineSprites,
            ExvsCommonAssets.ExvsMapSelectSprites => AssetFileType.MapSelectSprites,
            ExvsCommonAssets.ExvsArcadeSelectSmallSprites => AssetFileType.ArcadeSelectSmallSprites,
            ExvsCommonAssets.ExvsArcadeSelectUnitNameSprites => AssetFileType.ArcadeSelectUnitNameSprites,
            ExvsCommonAssets.ExvsCameraConfigs => AssetFileType.CameraConfigs,
            ExvsCommonAssets.ExvsCommonEffects => AssetFileType.CommonEffects,
            ExvsCommonAssets.ExvsCommonEffectParticles => AssetFileType.CommonEffectParticles,
            ExvsCommonAssets.ExvsCosmeticInfo => AssetFileType.CosmeticInfo,
            ExvsCommonAssets.ExvsTextStrings => AssetFileType.TextStrings,
            ExvsCommonAssets.ExvsSeriesLogoSprites => AssetFileType.SeriesLogoSprites,
            ExvsCommonAssets.ExvsSeriesLogoSprites2 => AssetFileType.SeriesLogoSprites2,
            _ => AssetFileType.Unknown
        };
    }
}
