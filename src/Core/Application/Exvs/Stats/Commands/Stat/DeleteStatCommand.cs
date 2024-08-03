using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Commands.Stat;

public record DeleteStatCommand(Guid Id) : IRequest;

public class DeleteStatCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<DeleteStatCommand>
{
    public async ValueTask<Unit> Handle(DeleteStatCommand request, CancellationToken cancellationToken)
    {
        await applicationDbContext.Stats
            .Where(entity => entity.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}
