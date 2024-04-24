namespace BoostStudio.Domain.Entities.Unit.Stats;

public class StatSet : BaseEntity<Guid>
{
    public int Order { get; set; }

    public ICollection<Stat> Stat { get; set; } = [];
    
    public uint UnitId { get; set; }
    
    public Unit Unit { get; set; } = null!;
}
