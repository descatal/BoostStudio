using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;

public record DeleteHitboxByHashCommand(uint Hash) : IRequest;

public class DeleteHitboxByHashCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<DeleteHitboxByHashCommand>
{
    public async ValueTask<Unit> Handle(DeleteHitboxByHashCommand request, CancellationToken cancellationToken)
    {
        await applicationDbContext.Hitboxes.Where(projectile => projectile.Hash == request.Hash).ExecuteDeleteAsync(cancellationToken);
        return Unit.Value;       
    }
}
