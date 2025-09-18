using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record DeleteAmmoCommand(uint Hash) : IRequest;

public class DeleteAmmoCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<DeleteAmmoCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteAmmoCommand command,
        CancellationToken cancellationToken
    )
    {
        var ammo = await applicationDbContext
            .Ammo.Where(ammo => ammo.Hash == command.Hash)
            .ToListAsync(cancellationToken: cancellationToken);

        applicationDbContext.Ammo.RemoveRange(ammo);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
