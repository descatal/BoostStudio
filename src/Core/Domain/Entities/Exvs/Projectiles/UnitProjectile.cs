namespace BoostStudio.Domain.Entities.Exvs.Projectiles;

public class UnitProjectile : BaseEntity<Guid>
{ 
    public uint GameUnitId { get; set; }

    public Units.Unit? Unit { get; set; }
    
    // the original first four bytes of the stats file 
    public uint? FileSignature { get; set; }

    public ICollection<Projectile> Projectiles { get; set; } = [];
}
