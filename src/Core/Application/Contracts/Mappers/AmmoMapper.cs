using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;
using AmmoEntity = BoostStudio.Domain.Entities.Unit.Ammo.Ammo;

namespace BoostStudio.Application.Contracts.Mappers;

[Mapper]
public partial class AmmoMapper
{    
    public partial IQueryable<AmmoDto> ProjectToDto(IQueryable<AmmoEntity> entity);
    
    public partial AmmoEntity AmmoToAmmo(AmmoEntity ammo);
    
    public partial AmmoEntity AmmoDetailsDtoToAmmo(AmmoDetailsDto ammo);
    
    [MapperIgnoreSource(nameof(AmmoEntity.Id))]
    [MapperIgnoreTarget(nameof(AmmoEntity.Id))]
    [MapperIgnoreSource(nameof(AmmoEntity.Hash))]
    [MapperIgnoreTarget(nameof(AmmoEntity.Hash))]
    public partial void AmmoToAmmo(AmmoEntity source, AmmoEntity destination);
 
    public partial AmmoEntity AmmoDtoToAmmo(AmmoDto ammo);

    public partial List<AmmoEntity> AmmoDtoToAmmo(List<AmmoDto> ammo);
    
    public partial AmmoDto AmmoToAmmoDto(AmmoEntity ammo);
    
    public partial AmmoEntity AmmoPropertiesBodyToAmmo(AmmoBinaryFormat.AmmoPropertiesBody ammoPropertiesBody);

    public List<AmmoEntity> AmmoBinaryFormatToAmmo(AmmoBinaryFormat ammoBinaryFormat)
    {
        return ammoBinaryFormat.Ammo.Select(ammoBody =>
        {
            var ammo = AmmoPropertiesBodyToAmmo(ammoBody.AmmoProperties);
            ammo.Hash = ammoBody.Hash;
            return ammo;
        }).ToList();
    }
}
