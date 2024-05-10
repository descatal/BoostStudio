using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Entities.Unit;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Mappers;

[Mapper]
public partial class UnitAmmoSlotMapper
{
    public partial IQueryable<UnitAmmoSlotDto> ProjectToDto(IQueryable<UnitAmmoSlot> entity);
    
    public partial UnitAmmoSlotDto UnitAmmoSlotToUnitAmmoSlotDto(UnitAmmoSlot unitAmmoSlot);
}
