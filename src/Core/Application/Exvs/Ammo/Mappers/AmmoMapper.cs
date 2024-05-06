using BoostStudio.Application.Exvs.Ammo.Commands;
using BoostStudio.Application.Exvs.Ammo.Models;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Exvs.Ammo.Mappers;

[Mapper]
public partial class AmmoMapper
{
    public partial Domain.Entities.Unit.Ammo.Ammo AmmoToAmmo(Domain.Entities.Unit.Ammo.Ammo ammo);
    
    public partial Domain.Entities.Unit.Ammo.Ammo CreateAmmoDtoToAmmo(CreateAmmoDto createAmmoCommand);
 
    public partial List<Domain.Entities.Unit.Ammo.Ammo> CreateAmmoDtoToAmmo(List<CreateAmmoDto> createAmmoCommand);
    
    public partial AmmoDto AmmoToAmmoDto(Domain.Entities.Unit.Ammo.Ammo ammo);
    
    public partial Domain.Entities.Unit.Ammo.Ammo AmmoPropertiesBodyToAmmo(AmmoBinaryFormat.AmmoPropertiesBody ammoPropertiesBody);

    public List<Domain.Entities.Unit.Ammo.Ammo> AmmoBinaryFormatToAmmo(AmmoBinaryFormat ammoBinaryFormat)
    {
        return ammoBinaryFormat.Ammo.Select(ammoBody =>
        {
            var ammo = AmmoPropertiesBodyToAmmo(ammoBody.AmmoProperties);
            ammo.Hash = ammoBody.Hash;
            return ammo;
            return new Domain.Entities.Unit.Ammo.Ammo();
        }).ToList();
    }
}
