using BoostStudio.Domain.Entities.Unit.Stats;

namespace BoostStudio.Domain.Entities.Unit.Projectiles;

public class UnitProjectile : BaseEntity<Guid>
{ 
    public uint GameUnitId { get; set; }

    public Unit? Unit { get; set; }
    
    // the original first four bytes of the stats file 
    public uint? FileSignature { get; set; }

    public ICollection<Projectile> Projectiles { get; set; } = [];
}
