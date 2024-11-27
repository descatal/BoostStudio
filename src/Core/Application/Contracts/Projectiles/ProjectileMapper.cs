using BoostStudio.Domain.Entities.Exvs.Projectiles;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Projectiles;

[Mapper]
public static partial class ProjectileMapper
{
    public static partial IQueryable<ProjectileDto> ProjectToDto(IQueryable<Projectile> entity);
    
    [MapProperty([nameof(Projectile.UnitProjectile), nameof(Projectile.UnitProjectile.GameUnitId)], [nameof(ProjectileDto.UnitId)])]
    public static partial ProjectileDto MapToDto(Projectile entity);
    
    public static partial List<ProjectileDto> MapToDto(List<Projectile> entity);
    
    public static partial void MapToEntity(ProjectileDetailsDto source, Projectile destination);
    
    public static partial Projectile MapToEntity(ProjectileDetailsDto dto);
    
    public static Projectile MapToEntity(uint hash, ProjectileDetailsDto sourceDto, Projectile? destination = null)
    {
        var entity = destination ?? new Projectile();
        if (destination is not null)
            MapToEntity(sourceDto, entity);
        else
            entity = MapToEntity(sourceDto);
        
        entity.Hash = hash;

        // coalesce it to null if 0 is supplied
        if (entity.HitboxHash == 0)
            entity.HitboxHash = null;

        return entity;
    }
    
    public static partial void MapToEntity(ProjectileBinaryFormat.ProjectilePropertiesBody source, Projectile destination);
    
    public static partial Projectile MapToEntity(ProjectileBinaryFormat.ProjectilePropertiesBody binary);
    
    public static Projectile MapToEntity(uint hash, ProjectileBinaryFormat.ProjectilePropertiesBody source, Projectile? destination = null)
    {
        var entity = destination ?? new Projectile();
        if (destination is not null)
            MapToEntity(source, entity);
        else
            entity = MapToEntity(source);
        
        entity.Hash = hash;

        if (entity.HitboxHash == 0)
            entity.HitboxHash = null;
        
        return entity;
    }
}
