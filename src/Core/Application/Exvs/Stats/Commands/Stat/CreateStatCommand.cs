using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;
using Microsoft.EntityFrameworkCore;
using StatMapper = BoostStudio.Application.Contracts.Stats.StatMapper;

namespace BoostStudio.Application.Exvs.Stats.Commands.Stat;

public record CreateStatCommand() : StatDetailsDto, IRequest<Guid>;

public class CreateStatCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateStatCommand, Guid>
{
    public async ValueTask<Guid> Handle(
        CreateStatCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = StatMapper.MapToEntity(command);
        applicationDbContext.Stats.Add(entity);

        if (command.UnitId is not null)
        {
            var unitStat = await applicationDbContext.UnitStats.FirstOrDefaultAsync(
                unitStat => unitStat.GameUnitId == command.UnitId,
                cancellationToken
            );

            if (unitStat is not null)
                entity.UnitStatId = unitStat.Id;
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
