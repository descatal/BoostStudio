using System.Collections;
using BoostStudio.Formats;

namespace BoostStudio.Infrastructure.Common.Constants;

public static class HitboxProperties
{
    public static List<HitboxBinaryFormat.PropertiesEnum> HitboxPropertiesEnumOrder =
    [
        HitboxBinaryFormat.PropertiesEnum.HitboxType,
        HitboxBinaryFormat.PropertiesEnum.Damage,
        HitboxBinaryFormat.PropertiesEnum.Unk8,
        HitboxBinaryFormat.PropertiesEnum.DownValueThreshold,
        HitboxBinaryFormat.PropertiesEnum.YorukeValueThreshold,
        HitboxBinaryFormat.PropertiesEnum.Unk20,
        HitboxBinaryFormat.PropertiesEnum.Unk24,
        HitboxBinaryFormat.PropertiesEnum.DamageCorrection,
        HitboxBinaryFormat.PropertiesEnum.SpecialEffect,
        HitboxBinaryFormat.PropertiesEnum.HitEffect,
        HitboxBinaryFormat.PropertiesEnum.FlyDirection1,
        HitboxBinaryFormat.PropertiesEnum.FlyDirection2,
        HitboxBinaryFormat.PropertiesEnum.FlyDirection3,
        HitboxBinaryFormat.PropertiesEnum.EnemyCameraShakeMultiplier,
        HitboxBinaryFormat.PropertiesEnum.PlayerCameraShakeMultiplier,
        HitboxBinaryFormat.PropertiesEnum.Unk56,
        HitboxBinaryFormat.PropertiesEnum.KnockUpAngle,
        HitboxBinaryFormat.PropertiesEnum.KnockUpRange,
        HitboxBinaryFormat.PropertiesEnum.Unk68,
        HitboxBinaryFormat.PropertiesEnum.MultipleHitIntervalFrame,
        HitboxBinaryFormat.PropertiesEnum.MultipleHitCount,
        HitboxBinaryFormat.PropertiesEnum.EnemyStunDuration,
        HitboxBinaryFormat.PropertiesEnum.PlayerStunDuration,
        HitboxBinaryFormat.PropertiesEnum.HitVisualEffect,
        HitboxBinaryFormat.PropertiesEnum.HitVisualEffectSizeMultiplier,
        HitboxBinaryFormat.PropertiesEnum.HitSoundEffectHash,
        HitboxBinaryFormat.PropertiesEnum.Unk100,
        HitboxBinaryFormat.PropertiesEnum.FriendlyDamageFlag,
        HitboxBinaryFormat.PropertiesEnum.Unk108
    ];
}
