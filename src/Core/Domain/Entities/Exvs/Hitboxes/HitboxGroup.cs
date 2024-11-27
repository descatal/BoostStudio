namespace BoostStudio.Domain.Entities.Exvs.Hitboxes;

public class HitboxGroup : BaseEntity<Guid>
{
    // the original first four bytes of the stats file, repurposed as the group's identifier
    public uint Hash { get; set; }

    public ICollection<Hitbox> Hitboxes { get; set; } = [];
    
    public ICollection<Units.Unit> Units { get; set; } = [];
}
