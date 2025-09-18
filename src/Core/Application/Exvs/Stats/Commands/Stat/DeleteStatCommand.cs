using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Commands.Stat;

public record DeleteStatCommand(Guid Id) : IRequest;

public class DeleteStatCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<DeleteStatCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteStatCommand request,
        CancellationToken cancellationToken
    )
    {
        var stats = await applicationDbContext
            .Stats.Where(entity => entity.Id == request.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        applicationDbContext.Stats.RemoveRange(stats);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
