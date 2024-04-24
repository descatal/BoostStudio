using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Exvs.UnitStats.Commands.Models;
using StatDtoMapper=BoostStudio.Application.Exvs.UnitStats.Commands.Models.StatDtoMapper;

namespace BoostStudio.Application.Exvs.UnitStats.Commands;

public record UpdateStatSetCommand(Guid Id, int Order, ICollection<StatDto> Stat, uint UnitId) : IRequest;

public class UpdateStatSetCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateStatSetCommand>
{
    public async Task Handle(UpdateStatSetCommand command, CancellationToken cancellationToken)
    {
        var statSet = await applicationDbContext.StatSets.FindAsync([command.Id], cancellationToken: cancellationToken);
        Guard.Against.Null(statSet, nameof(statSet));

        statSet.Order = command.Order;
        statSet.Stat = new StatDtoMapper().StatDtoToStat(command.Stat.ToList());
        statSet.UnitId = command.UnitId;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
