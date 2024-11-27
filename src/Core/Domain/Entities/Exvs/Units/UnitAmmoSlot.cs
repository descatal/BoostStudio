using BoostStudio.Domain.Entities.Exvs.Stats;

namespace BoostStudio.Domain.Entities.Exvs.Units;

public class UnitAmmoSlot : BaseEntity<Guid>
{
    public int SlotOrder { get; set; }
    
    // Navigation properties to Ammo (AmmoHash is key to Ammo), to ensure the ammo is a valid ammo item
    public uint AmmoHash { get; set; }
    public Ammo.Ammo? Ammo { get; set; }
    
    public Guid UnitStatId { get; set; }
    
    public UnitStat? UnitStat { get; set; }
}
