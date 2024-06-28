using BoostStudio.Application.Contracts.Projectiles;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Hitboxes;

[Mapper]
public static partial class HitboxMapper
{
    public static partial IQueryable<HitboxDto> ProjectToDto(IQueryable<Hitbox> entity);
    
    public static partial HitboxDto MapToDto(Hitbox entity);
    
    public static partial List<HitboxDto> MapToDto(List<Hitbox> entity);
    
    public static partial void MapToEntity(HitboxDetailsDto source, Hitbox destination);
    
    public static partial Hitbox MapToEntity(HitboxDetailsDto dto);
    
    public static Hitbox MapToEntity(uint hash, HitboxDetailsDto sourceDto, Hitbox? destination = null)
    {
        var entity = destination ?? new Hitbox();
        if (destination is not null)
            MapToEntity(sourceDto, entity);
        else
            entity = MapToEntity(sourceDto);
        
        entity.Hash = hash;
        return entity;
    }
}
