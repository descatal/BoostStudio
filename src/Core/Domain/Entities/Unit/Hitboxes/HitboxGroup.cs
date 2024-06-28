namespace BoostStudio.Domain.Entities.Unit.Hitboxes;

public class HitboxGroup : BaseEntity<Guid>
{
    public uint? GameUnitId { get; set; }

    public Unit? Unit { get; set; }
    
    // the original first four bytes of the stats file, repurposed as the group's identifier
    public uint Hash { get; set; }

    public ICollection<Hitbox> Hitboxes { get; set; } = [];
}
