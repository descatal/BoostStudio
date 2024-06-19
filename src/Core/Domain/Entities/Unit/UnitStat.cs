using BoostStudio.Domain.Entities.Unit.Stats;

namespace BoostStudio.Domain.Entities.Unit;

public class UnitStat : BaseEntity<Guid>
{ 
    public uint GameUnitId { get; set; }

    public Unit? Unit { get; set; }
    
    // the original first four bytes of the stats file 
    public uint? FileSignature { get; set; }

    public ICollection<Stat> Stats { get; set; } = [];
    
    public ICollection<Ammo.Ammo> Ammo { get; set; } = [];
    
    public ICollection<UnitAmmoSlot> AmmoSlots { get; set; } = [];
}
