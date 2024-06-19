using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;
using Microsoft.EntityFrameworkCore;
using StatMapper=BoostStudio.Application.Contracts.Stats.StatMapper;

namespace BoostStudio.Application.Exvs.Stats.Commands.Stat;

public record UpdateStatCommand(Guid Id) : StatDetailsDto, IRequest;

public class UpdateStatCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateStatCommand>
{
    public async Task Handle(UpdateStatCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Stats
            .Include(stat => stat.UnitStat)
            .FirstOrDefaultAsync(statSet => statSet.Id == command.Id, cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(command.Id, existingEntity);
        StatMapper.MapToEntity(command.Id, command, existingEntity);

        if (existingEntity.UnitStat?.GameUnitId != command.UnitId)
        {
            var unitStat = await applicationDbContext.UnitStats
                .FirstOrDefaultAsync(unitStat => unitStat.GameUnitId == command.UnitId, cancellationToken);
            
            existingEntity.UnitStat = unitStat;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
