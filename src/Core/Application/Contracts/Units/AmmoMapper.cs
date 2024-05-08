using Riok.Mapperly.Abstractions;
using Unit = BoostStudio.Domain.Entities.Unit.Unit;

namespace BoostStudio.Application.Contracts.Units;

[Mapper]
public partial class UnitMapper
{
    public partial List<Unit> UnitDtoToUnit(List<UnitDto> dto);
    
    public partial Unit UnitDtoToUnit(UnitDto dto);
    
    public partial List<UnitDto> UnitToUnitDto(List<Unit> entity);
    
    public partial UnitDto UnitToUnitDto(Unit entity);
}
