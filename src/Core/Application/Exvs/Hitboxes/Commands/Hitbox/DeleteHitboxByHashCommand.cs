using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;

public record DeleteHitboxByHashCommand(uint Hash) : IRequest;

public class DeleteHitboxByHashCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<DeleteHitboxByHashCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteHitboxByHashCommand request,
        CancellationToken cancellationToken
    )
    {
        var hitboxes = await applicationDbContext
            .Hitboxes.Where(projectile => projectile.Hash == request.Hash)
            .ToListAsync(cancellationToken: cancellationToken);

        applicationDbContext.Hitboxes.RemoveRange(hitboxes);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
