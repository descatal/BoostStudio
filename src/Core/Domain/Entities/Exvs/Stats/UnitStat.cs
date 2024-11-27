using BoostStudio.Domain.Entities.Exvs.Units;

namespace BoostStudio.Domain.Entities.Exvs.Stats;

public class UnitStat : BaseEntity<Guid>
{ 
    public uint GameUnitId { get; set; }

    public Units.Unit? Unit { get; set; }
    
    // the original first four bytes of the stats file 
    public uint? FileSignature { get; set; }

    public ICollection<Stat> Stats { get; set; } = [];
    
    public ICollection<Ammo.Ammo> Ammo { get; set; } = [];
    
    public ICollection<UnitAmmoSlot> AmmoSlots { get; set; } = [];
}
