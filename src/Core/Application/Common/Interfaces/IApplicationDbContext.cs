﻿using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Domain.Entities.Unit.Projectiles;
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
    
    public DbSet<Projectile> Projectiles { get; }
    
    public DbSet<UnitProjectile> UnitProjectiles { get; }
    
    public DbSet<Hitbox> Hitboxes { get; }
    
    public DbSet<HitboxGroup> HitboxGroups { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
