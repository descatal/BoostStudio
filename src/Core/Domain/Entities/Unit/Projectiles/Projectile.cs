using BoostStudio.Domain.Entities.Unit.Hitboxes;

namespace BoostStudio.Domain.Entities.Unit.Projectiles;

public class Projectile : BaseEntity<Guid>
{
    public uint Hash { get; set; }

    public uint ProjectileType { get; set; }

    // navigation to hitbox properties, optional
    // if supplied, the hitbox will attach to the projectile model
    public uint? HitboxHash { get; set; }
    public Hitbox? Hitbox { get; set; }

    // determine where the projectile is spawned
    public uint ModelHash { get; set; }
    public uint SkeletonIndex { get; set; }

    public uint AimType { get; set; }
    public float TranslateY { get; set; }
    public float TranslateZ { get; set; }
    public float TranslateX { get; set; }
    public float RotateX { get; set; }
    public float RotateZ { get; set; }

    // this can be turned into navigation in the future
    public uint CosmeticHash { get; set; }
    public uint Unk44 { get; set; }
    public uint Unk48 { get; set; }
    public uint Unk52 { get; set; }
    public uint Unk56 { get; set; }
    public uint AmmoConsumption { get; set; }
    public uint DurationFrame { get; set; }
    public float MaxTravelDistance { get; set; }
    public float InitialSpeed { get; set; }
    public float Acceleration { get; set; }
    public uint AccelerationStartFrame { get; set; }
    public float Unk84 { get; set; }
    public float MaxSpeed { get; set; }
    public float Reserved92 { get; set; }
    public float Reserved96 { get; set; }
    public float Reserved100 { get; set; }
    public float Reserved104 { get; set; }
    public float Reserved108 { get; set; }
    public float Reserved112 { get; set; }
    public uint Reserved116 { get; set; }
    public float HorizontalGuidance { get; set; }
    public float HorizontalGuidanceAngle { get; set; }
    public float VerticalGuidance { get; set; }
    public float VerticalGuidanceAngle { get; set; }
    public uint Reserved136 { get; set; }
    public uint Reserved140 { get; set; }
    public float Reserved144 { get; set; }
    public float Reserved148 { get; set; }
    public float Reserved152 { get; set; }
    public float Reserved156 { get; set; }
    public float Reserved160 { get; set; }
    public float Reserved164 { get; set; }
    public uint Reserved168 { get; set; }
    public float Reserved172 { get; set; }
    public float Size { get; set; }
    public uint Reserved180 { get; set; }
    public uint Reserved184 { get; set; }
    public uint SoundEffectHash { get; set; }
    public uint Reserved192 { get; set; }
    public uint Reserved196 { get; set; }
    public uint ChainedProjectileHash { get; set; }
    public uint Reserved204 { get; set; }
    public uint Reserved208 { get; set; }
    public uint Reserved212 { get; set; }
    public uint Reserved216 { get; set; }
    public float Reserved220 { get; set; }
    public float Reserved224 { get; set; }
    public float Reserved228 { get; set; }
    public float Reserved232 { get; set; }
    public float Reserved236 { get; set; }
    public float Reserved240 { get; set; }
    public float Reserved244 { get; set; }
    public float Reserved248 { get; set; }
    public float Reserved252 { get; set; }
    public float Reserved256 { get; set; }
    public float Reserved260 { get; set; }
    public float Reserved264 { get; set; }
    public float Reserved268 { get; set; }
    public float Reserved272 { get; set; }
    public float Reserved276 { get; set; }
    
    public Guid? UnitProjectileId { get; set; }

    public UnitProjectile? UnitProjectile { get; set; }
}
