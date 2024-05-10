using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;
using Microsoft.EntityFrameworkCore;
using StatMapper=BoostStudio.Application.Contracts.Mappers.StatMapper;

namespace BoostStudio.Application.Exvs.Stats.Commands;

public record UpdateStatCommand(Guid Id, StatDto Stat, Guid? UnitStatId = null, int? Order = null) : IRequest;

public class UpdateStatCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateStatCommand>
{
    public async Task Handle(UpdateStatCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Stats
            .FirstOrDefaultAsync(statSet => statSet.Id == command.Id, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.Id, existingEntity);

        var mapper = new StatMapper();
        existingEntity = mapper.StatDtoToStat(command.Stat);
        
        existingEntity.Order = command.Order ?? 0;
        existingEntity.UnitStatId = command.UnitStatId;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
