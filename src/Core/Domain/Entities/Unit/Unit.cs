using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Domain.Entities.Unit.Projectiles;

namespace BoostStudio.Domain.Entities.Unit;

public class Unit : BaseEntity<Guid>
{
    // The Id that's used in game, e.g. 1011 for Gundam
    public uint GameUnitId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string NameJapanese { get; set; } = string.Empty;
    
    public string NameChinese { get; set; } = string.Empty;
    
    public UnitStat? UnitStats { get; set; }
    
    public UnitProjectile? UnitProjectiles { get; set; }
    
    public uint? HitboxGroupHash { get; set; }
    public HitboxGroup? HitboxGroup { get; set; }
}
