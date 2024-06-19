using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;
using AmmoEntity = BoostStudio.Domain.Entities.Unit.Ammo.Ammo;

namespace BoostStudio.Application.Contracts.Ammo;

[Mapper]
public static partial class AmmoMapper
{    
    public static partial IQueryable<AmmoDto> ProjectToDto(IQueryable<AmmoEntity> entity);
    
    public static partial AmmoEntity AmmoToAmmo(AmmoEntity ammo);
    
    public static partial AmmoEntity AmmoDetailsDtoToAmmo(AmmoDetailsDto ammo);
    
    [MapperIgnoreSource(nameof(AmmoEntity.Id))]
    [MapperIgnoreTarget(nameof(AmmoEntity.Id))]
    [MapperIgnoreSource(nameof(AmmoEntity.Hash))]
    [MapperIgnoreTarget(nameof(AmmoEntity.Hash))]
    public static partial void AmmoToAmmo(AmmoEntity source, AmmoEntity destination);
 
    public static partial AmmoEntity AmmoDtoToAmmo(AmmoDto ammo);

    public static partial List<AmmoEntity> AmmoDtoToAmmo(List<AmmoDto> ammo);
    
    public static partial AmmoDto AmmoToAmmoDto(AmmoEntity ammo);
    
    public static partial AmmoEntity AmmoPropertiesBodyToAmmo(AmmoBinaryFormat.AmmoPropertiesBody ammoPropertiesBody);

    public static List<AmmoEntity> AmmoBinaryFormatToAmmo(AmmoBinaryFormat ammoBinaryFormat)
    {
        return ammoBinaryFormat.Ammo.Select(ammoBody =>
        {
            var ammo = AmmoPropertiesBodyToAmmo(ammoBody.AmmoProperties);
            ammo.Hash = ammoBody.Hash;
            return ammo;
        }).ToList();
    }
    
    public static partial void MapToEntity(AmmoDetailsDto source, AmmoEntity destination);
    
    public static partial AmmoEntity MapToEntity(AmmoDetailsDto dto);
    
    public static AmmoEntity MapToEntity(uint hash, AmmoDetailsDto sourceDto, AmmoEntity? destination = null)
    {
        var entity = destination ?? new AmmoEntity();
        if (destination is not null)
            MapToEntity(sourceDto, entity);
        else
            entity = MapToEntity(sourceDto);
        
        entity.Hash = hash;
        return entity;
    }
}
