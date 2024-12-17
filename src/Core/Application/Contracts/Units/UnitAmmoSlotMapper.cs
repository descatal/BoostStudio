using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Domain.Entities.Exvs.Units;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Units;

[Mapper]
public partial class UnitAmmoSlotMapper
{
    public partial IQueryable<UnitAmmoSlotDto> ProjectToDto(IQueryable<UnitAmmoSlot> entity);
    
    public partial UnitAmmoSlotDto UnitAmmoSlotToUnitAmmoSlotDto(UnitAmmoSlot unitAmmoSlot);
}
