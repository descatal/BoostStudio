using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;

public record CreateHitboxCommand() : HitboxDetailsDto, IRequest<Guid>;

public class CreateProjectileCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<CreateHitboxCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateHitboxCommand command, CancellationToken cancellationToken)
    {
        var hitboxGroupHash = await applicationDbContext.HitboxGroups
            .FirstOrDefaultAsync(group => group.Hash == command.HitboxGroupHash, cancellationToken);

        Guard.Against.NotFound(command.HitboxGroupHash, hitboxGroupHash);
        
        var entity = HitboxMapper.MapToEntity(command);
        entity.Hash = (uint)(new Random().Next());
        applicationDbContext.Hitboxes.Add(entity);
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
