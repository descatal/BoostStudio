using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Unit;

namespace BoostStudio.Application.Exvs.UnitStats.Commands;

public record CreateUnitStatCommand(
    uint GameUnitId
) : IRequest<Guid>;

public class CreateUnitStatCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateUnitStatCommand, Guid>
{
    public async Task<Guid> Handle(CreateUnitStatCommand command, CancellationToken cancellationToken)
    {
        var entity = new UnitStat
        {
            GameUnitId = command.GameUnitId
        };
        
        applicationDbContext.UnitStats.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

