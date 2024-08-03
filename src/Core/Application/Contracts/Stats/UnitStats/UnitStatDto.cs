using BoostStudio.Application.Contracts.Ammo;

namespace BoostStudio.Application.Contracts.Stats.UnitStats;

public record UnitStatDto
{
    public string? Id { get; set; }

    public uint UnitId { get; set; }

    // public string NameEnglish { get; set; } = string.Empty;
    //
    // public string NameJapanese { get; set; } = string.Empty;
    //
    // public string NameChinese { get; set; } = string.Empty;

    // public List<StatDto> Stats { get; set; } = [];
    
    public List<UnitAmmoSlotDto> AmmoSlots { get; set; } = [];
}
