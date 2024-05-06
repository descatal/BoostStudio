using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Stats;

[Mapper]
public partial class StatMapper
{
    public partial StatDto StatToStatDto(Stat stat);
    public partial List<StatDto> StatToStatDto(List<Stat> stat);
    
    public partial Stat StatDtoToStat(StatDto statDto);
    
    public partial List<Stat> StatDtoToStat(List<StatDto> statDto);
}
