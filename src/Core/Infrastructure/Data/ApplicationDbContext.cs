using System.Reflection;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Ammo;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using BoostStudio.Domain.Entities.Unit.Stats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : DbContext(options), IApplicationDbContext
{
    public DbSet<Config> Configs => Set<Config>();
    
    public DbSet<Unit> Units => Set<Unit>();
    
    public DbSet<UnitStat> UnitStats => Set<UnitStat>();
    
    public DbSet<UnitAmmoSlot> UnitAmmoSlots => Set<UnitAmmoSlot>();
    
    public DbSet<Stat> Stats => Set<Stat>();
    
    public DbSet<Ammo> Ammo => Set<Ammo>();
    
    public DbSet<UnitProjectile> UnitProjectiles => Set<UnitProjectile>();
    
    public DbSet<Projectile> Projectiles => Set<Projectile>();
    
    public DbSet<Hitbox> Hitboxes => Set<Hitbox>();
    
    public DbSet<HitboxGroup> HitboxGroups => Set<HitboxGroup>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
