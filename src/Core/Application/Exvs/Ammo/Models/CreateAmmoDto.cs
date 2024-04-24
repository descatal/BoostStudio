namespace BoostStudio.Application.Exvs.Ammo.Models;

public record CreateAmmoDto
{
    public uint AmmoType { get; init; }
    public uint MaxAmmo { get; init; }
    public uint InitialAmmo { get; init; }
    public uint TimedDurationFrame { get; init; }
    public uint Unk16 { get; init; }
    public uint ReloadType { get; init; }
    public uint CooldownDurationFrame { get; init; }
    public uint ReloadDurationFrame { get; init; }
    public uint AssaultBurstReloadDurationFrame { get; init; }
    public uint BlastBurstReloadDurationFrame { get; init; }
    public uint Unk40 { get; init; }
    public uint Unk44 { get; init; }
    public uint InactiveUnk48 { get; init; }
    public uint InactiveCooldownDurationFrame { get; init; }
    public uint InactiveReloadDurationFrame { get; init; }
    public uint InactiveAssaultBurstReloadDurationFrame { get; init; }
    public uint InactiveBlastBurstReloadDurationFrame { get; init; }
    public uint InactiveUnk68 { get; init; }
    public uint InactiveUnk72 { get; init; }
    public uint BurstReplenish { get; init; }
    public uint Unk80 { get; init; }
    public uint Unk84 { get; init; }
    public uint Unk88 { get; init; }
    public uint ChargeInput { get; init; }
    public uint ChargeDurationFrame { get; init; }
    public uint AssaultBurstChargeDurationFrame { get; init; }
    public uint BlastBurstChargeDurationFrame { get; init; }
    public uint Unk108 { get; init; }
    public uint Unk112 { get; init; }
    public uint ReleaseChargeLingerDurationFrame { get; init; }
    public uint MaxChargeLevel { get; init; }
    public uint Unk124 { get; init; }
    public uint Unk128 { get; init; }
    public long? Hash { get; set; }
}
