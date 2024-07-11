using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using HitboxGroupEntity = BoostStudio.Domain.Entities.Unit.Hitboxes.HitboxGroup;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record UpdateHitboxGroupCommand(uint Hash, uint? UnitId = null) : IRequest;

public class UpdateHitboxGroupCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateHitboxGroupCommand>
{
    public async ValueTask<Unit> Handle(UpdateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        var entity = applicationDbContext.HitboxGroups
            .FirstOrDefault(group => group.Hash == command.Hash);

        Guard.Against.NotFound(command.Hash, entity);

        entity.GameUnitId = command.UnitId;
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}

