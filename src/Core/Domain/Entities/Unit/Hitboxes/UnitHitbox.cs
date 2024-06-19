namespace BoostStudio.Domain.Entities.Unit.Hitboxes;

public class UnitHitbox : BaseEntity<Guid>
{
    public uint GameUnitId { get; set; }

    public Unit? Unit { get; set; }
    
    // the original first four bytes of the stats file 
    public uint? FileSignature { get; set; }

    public ICollection<Hitbox> Hitboxes { get; set; } = [];
}
