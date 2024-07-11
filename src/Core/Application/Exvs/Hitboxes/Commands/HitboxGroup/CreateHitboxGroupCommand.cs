using BoostStudio.Application.Common.Interfaces;
using HitboxGroupEntity = BoostStudio.Domain.Entities.Unit.Hitboxes.HitboxGroup;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record CreateHitboxGroupCommand(uint Hash, uint? UnitId = null) : IRequest;

public class CreateHitboxGroupCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateHitboxGroupCommand>
{
    public async ValueTask<Unit> Handle(CreateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        var entity = new HitboxGroupEntity
        {
            Hash = command.Hash, GameUnitId = command.UnitId,
        };
        applicationDbContext.HitboxGroups.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}

