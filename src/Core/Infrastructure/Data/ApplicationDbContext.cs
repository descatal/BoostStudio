using System.Reflection;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Hitboxes;
using BoostStudio.Domain.Entities.Exvs.Projectiles;
using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Domain.Entities.Exvs.Stats;
using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Domain.Entities.Exvs.Units;
using Microsoft.EntityFrameworkCore;
using AmmoEntity = BoostStudio.Domain.Entities.Exvs.Ammo.Ammo;

namespace BoostStudio.Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : DbContext(options), IApplicationDbContext
{
    public DbSet<Config> Configs => Set<Config>();
    
    public DbSet<Unit> Units => Set<Unit>();

    public DbSet<Series> Series => Set<Series>();

    public DbSet<PlayableSeries> PlayableSeries => Set<PlayableSeries>();

    public DbSet<UnitStat> UnitStats => Set<UnitStat>();
    
    public DbSet<UnitAmmoSlot> UnitAmmoSlots => Set<UnitAmmoSlot>();
    
    public DbSet<Stat> Stats => Set<Stat>();
    
    public DbSet<AmmoEntity> Ammo => Set<AmmoEntity>();
    
    public DbSet<UnitProjectile> UnitProjectiles => Set<UnitProjectile>();
    
    public DbSet<Projectile> Projectiles => Set<Projectile>();
    
    public DbSet<Hitbox> Hitboxes => Set<Hitbox>();
    
    public DbSet<HitboxGroup> HitboxGroups => Set<HitboxGroup>();
    
    public DbSet<AssetFile> AssetFiles => Set<AssetFile>();
    
    public DbSet<PatchFile> PatchFiles => Set<PatchFile>();

    public DbSet<Tbl> Tbl => Set<Tbl>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
