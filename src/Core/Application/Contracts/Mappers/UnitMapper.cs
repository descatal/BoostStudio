using BoostStudio.Application.Contracts.Units;
using Riok.Mapperly.Abstractions;
using Unit = BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Contracts.Mappers;

[Mapper]
public static partial class UnitMapper
{
    public static partial List<Unit> UnitDtoToUnit(List<UnitDto> dto);
    
    [MapProperty(nameof(UnitDto.UnitId), nameof(Unit.GameUnitId))]
    public static partial Unit MapToEntity(UnitDto dto);
    
    public static partial List<UnitDto> MapToDto(List<Unit> entity);
    
    [MapProperty(nameof(Unit.GameUnitId), nameof(UnitDto.UnitId))]
    public static partial UnitDto MapToDto(Unit entity);

    public static partial Unit Update(Unit entity);
}
