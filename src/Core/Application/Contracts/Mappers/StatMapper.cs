using BoostStudio.Application.Contracts.Stats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Mappers;

[Mapper]
public partial class StatMapper
{
    public partial IQueryable<StatDto> ProjectToDto(IQueryable<Stat> entity);
    
    public partial StatDto StatToStatDto(Stat stat);
    public partial List<StatDto> StatToStatDto(List<Stat> stat);
    
    public partial Stat StatDtoToStat(StatDto statDto);
    
    public partial List<Stat> StatDtoToStat(List<StatDto> statDto);
}
