using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;

namespace BoostStudio.Application.Exvs.Stats.Commands;

public record CreateStatCommand(StatDto Stat, Guid? UnitStatId = null, int? Order = null) : IRequest<Guid>;

public class CreateStatCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<CreateStatCommand, Guid>
{
    public async Task<Guid> Handle(CreateStatCommand command, CancellationToken cancellationToken)
    {
        var mapper = new StatMapper();
        var entity = mapper.StatDtoToStat(command.Stat);
        
        entity.Order = command.Order ?? 0;
        entity.UnitStatId = command.UnitStatId;
        
        applicationDbContext.Stats.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
