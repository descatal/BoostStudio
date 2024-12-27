using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Hitboxes;
using BoostStudio.Domain.Entities.Exvs.Projectiles;
using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Domain.Entities.Exvs.Stats;
using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Domain.Entities.Exvs.Units;
using BoostStudio.Domain.Entities.Exvs.Units.Characters;
using Microsoft.EntityFrameworkCore;
using Unit=BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Config> Configs { get; }
    
    public DbSet<Unit> Units { get; }

    public DbSet<Series> Series { get; }

    public DbSet<PlayableCharacter> PlayableCharacters { get; }

    public DbSet<PlayableSeries> PlayableSeries { get; }

    public DbSet<UnitStat> UnitStats { get; }
    
    public DbSet<UnitAmmoSlot> UnitAmmoSlots { get; }
    
    public DbSet<Stat> Stats { get; }
    
    public DbSet<Ammo> Ammo { get; }
    
    public DbSet<Projectile> Projectiles { get; }
    
    public DbSet<UnitProjectile> UnitProjectiles { get; }
    
    public DbSet<Hitbox> Hitboxes { get; }
    
    public DbSet<HitboxGroup> HitboxGroups { get; }
    
    public DbSet<AssetFile> AssetFiles { get; }
    
    public DbSet<PatchFile> PatchFiles { get; }
    
    public DbSet<Tbl> Tbl { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
