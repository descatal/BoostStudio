namespace BoostStudio.Application.Contracts.Hitboxes;

public record HitboxDto : HitboxDetailsDto
{
    public uint Hash { get; set; }
}

public record HitboxDetailsDto
{
    public uint HitboxType { get; set; }
    
    public uint Damage { get; set; }
    
    public uint Unk8 { get; set; }
    
    public uint DownValueThreshold { get; set; }
    
    public uint YorukeValueThreshold { get; set; }
    
    public uint Unk20 { get; set; }
    
    public uint Unk24 { get; set; }
    
    public float DamageCorrection { get; set; }
    
    public uint SpecialEffect { get; set; }
    
    public uint HitEffect { get; set; }
    
    public uint FlyDirection1 { get; set; }
    
    public uint FlyDirection2 { get; set; }
    
    public uint FlyDirection3 { get; set; }
    
    public uint EnemyCameraShakeMultiplier { get; set; }
    
    public uint PlayerCameraShakeMultiplier { get; set; }
    
    public uint Unk56 { get; set; }
    
    public uint KnockUpAngle { get; set; }
    
    public uint KnockUpRange { get; set; }
    
    public uint Unk68 { get; set; }
    
    public uint MultipleHitIntervalFrame { get; set; }
    
    public uint MultipleHitCount { get; set; }
    
    public uint EnemyStunDuration { get; set; }
    
    public uint PlayerStunDuration { get; set; }
    
    public uint HitVisualEffect { get; set; }
    
    public float HitVisualEffectSizeMultiplier { get; set; }
    
    public uint HitSoundEffectHash { get; set; }
    
    public uint Unk100 { get; set; }
    
    public uint FriendlyDamageFlag { get; set; }
    
    public uint Unk108 { get; set; }
    
    public uint HitboxGroupHash { get; set; }
}
