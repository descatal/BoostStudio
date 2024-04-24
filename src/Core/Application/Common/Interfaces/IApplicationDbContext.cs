using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Stats;
using Microsoft.EntityFrameworkCore;
using Unit=BoostStudio.Domain.Entities.Unit.Unit;

namespace BoostStudio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Unit> Units { get; }
    
    public DbSet<Ammo> Ammo { get; }

    public DbSet<StatSet> StatSets { get; }
    
    public DbSet<Stat> Stats { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
