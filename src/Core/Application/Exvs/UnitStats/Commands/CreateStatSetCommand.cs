using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Exvs.UnitStats.Commands.Models;
using BoostStudio.Domain.Entities.Unit.Stats;
using StatDtoMapper=BoostStudio.Application.Exvs.UnitStats.Commands.Models.StatDtoMapper;

namespace BoostStudio.Application.Exvs.UnitStats.Commands;

public record CreateStatSetCommand(int Order, ICollection<StatDto> Stat, uint UnitId) : IRequest;

public class CreateStatSetCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateStatSetCommand>
{
    public async Task Handle(CreateStatSetCommand command, CancellationToken cancellationToken)
    {
        applicationDbContext.StatSets.Add(new StatSet
        {
            Order = command.Order,
            Stat = new StatDtoMapper().StatDtoToStat(command.Stat.ToList()),
            UnitId = command.UnitId
        });
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
