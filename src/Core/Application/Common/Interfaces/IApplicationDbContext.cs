using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Stats;
using Microsoft.EntityFrameworkCore;
using Unit=BoostStudio.Domain.Entities.Unit.Unit;

namespace BoostStudio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Unit> Units { get; }
    
    public DbSet<UnitStat> UnitStats { get; }
    
    public DbSet<UnitAmmoSlot> UnitAmmoSlots { get; }
    
    public DbSet<Stat> Stats { get; }
    
    public DbSet<Ammo> Ammo { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
