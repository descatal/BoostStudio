﻿namespace BoostStudio.Application.Contracts.Stats.UnitStats;

public class UnitAmmoSlotDto
{
    public string? Id { get; set; }
    
    public int SlotOrder { get; set; }
    
    public uint AmmoHash { get; set; }
}
