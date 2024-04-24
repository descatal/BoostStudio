using BoostStudio.Domain.Entities.Unit.Stats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Exvs.UnitStats.Commands.Models;

[Mapper]
public partial class StatDtoMapper
{
    public partial List<StatDto> StatToStatDto(List<Stat> stat);
    
    public partial List<Stat> StatDtoToStat(List<StatDto> statDto);
}
