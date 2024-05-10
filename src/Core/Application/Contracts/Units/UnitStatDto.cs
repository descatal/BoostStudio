using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Application.Contracts.Stats;

namespace BoostStudio.Application.Contracts.Units;

public record UnitStatDto
{
    public string? Id { get; set; }

    public uint GameUnitId { get; set; }

    public string NameEnglish { get; set; } = string.Empty;
    
    public string NameJapanese { get; set; } = string.Empty;
    
    public string NameChinese { get; set; } = string.Empty;

    public List<StatDto> Stats { get; set; } = [];
    
    public List<AmmoDto> Ammo { get; set; } = [];
    
    public List<UnitAmmoSlotDto> AmmoSlots { get; set; } = [];
}
