using System.Reflection;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Stats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Ammo> Ammo => Set<Ammo>();
    
    public DbSet<StatSet> StatSets => Set<StatSet>();
    
    public DbSet<Stat> Stats => Set<Stat>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
