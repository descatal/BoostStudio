using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Ammo;

[Mapper]
public partial class AmmoMapper
{
    public partial Domain.Entities.Unit.Ammo.Ammo AmmoToAmmo(Domain.Entities.Unit.Ammo.Ammo ammo);
    
    public partial Domain.Entities.Unit.Ammo.Ammo AmmoDtoToAmmo(AmmoDto createAmmoCommand);
 
    public partial List<Domain.Entities.Unit.Ammo.Ammo> AmmoDtoToAmmo(List<AmmoDto> createAmmoCommand);
    
    public partial AmmoDto AmmoToAmmoDto(Domain.Entities.Unit.Ammo.Ammo ammo);
    
    public partial Domain.Entities.Unit.Ammo.Ammo AmmoPropertiesBodyToAmmo(AmmoBinaryFormat.AmmoPropertiesBody ammoPropertiesBody);

    public List<Domain.Entities.Unit.Ammo.Ammo> AmmoBinaryFormatToAmmo(AmmoBinaryFormat ammoBinaryFormat)
    {
        return ammoBinaryFormat.Ammo.Select(ammoBody =>
        {
            var ammo = AmmoPropertiesBodyToAmmo(ammoBody.AmmoProperties);
            ammo.Hash = ammoBody.Hash;
            return ammo;
        }).ToList();
    }
}
