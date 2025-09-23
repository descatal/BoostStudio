using BoostStudio.Domain.Entities.Exvs.Stats;

namespace BoostStudio.Domain.Entities.Exvs.Ammo;

public class Ammo : BaseEntity<Guid>
{
    public uint Hash { get; set; }
    public uint AmmoType { get; set; }
    public uint MaxAmmo { get; set; }
    public uint InitialAmmo { get; set; }
    public uint TimedDurationFrame { get; set; }
    public uint Unk16 { get; set; }
    public uint ReloadType { get; set; }
    public uint CooldownDurationFrame { get; set; }
    public uint ReloadDurationFrame { get; set; }
    public uint AssaultBurstReloadDurationFrame { get; set; }
    public uint BlastBurstReloadDurationFrame { get; set; }
    public uint Unk40 { get; set; }
    public uint Unk44 { get; set; }
    public uint InactiveUnk48 { get; set; }
    public uint InactiveCooldownDurationFrame { get; set; }
    public uint InactiveReloadDurationFrame { get; set; }
    public uint InactiveAssaultBurstReloadDurationFrame { get; set; }
    public uint InactiveBlastBurstReloadDurationFrame { get; set; }
    public uint InactiveUnk68 { get; set; }
    public uint InactiveUnk72 { get; set; }
    public uint BurstReplenish { get; set; }
    public uint Unk80 { get; set; }
    public uint Unk84 { get; set; }
    public uint Unk88 { get; set; }
    public uint ChargeInput { get; set; }
    public uint ChargeDurationFrame { get; set; }
    public uint AssaultBurstChargeDurationFrame { get; set; }
    public uint BlastBurstChargeDurationFrame { get; set; }
    public uint Unk108 { get; set; }
    public uint Unk112 { get; set; }
    public uint ReleaseChargeLingerDurationFrame { get; set; }
    public uint MaxChargeLevel { get; set; }
    public uint Unk124 { get; set; }
    public uint ChargeMultiLockFlag { get; set; }

    // Optional navigation, can be used to assign this ammo to a UnitStat entry
    public int Order { get; set; }

    public Guid? UnitStatId { get; set; }

    public UnitStat? UnitStat { get; set; }
}
