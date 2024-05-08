using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Application.Contracts.Stats;

namespace BoostStudio.Application.Contracts.Units;

public record UnitStatDto
{
    public uint GameUnitId { get; set; }

    public List<StatDto> Stats { get; set; } = [];
    
    public List<AmmoDto> Ammo { get; set; } = [];
    
    public List<UnitAmmoSlotDto> AmmoSlots { get; set; } = [];
}
