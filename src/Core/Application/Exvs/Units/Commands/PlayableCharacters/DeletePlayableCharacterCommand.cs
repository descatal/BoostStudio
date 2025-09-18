using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Commands.PlayableCharacters;

public record DeletePlayableCharacterCommand(uint UnitId) : IRequest;

public class DeletePlayableCharacterCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<DeletePlayableCharacterCommand>
{
    public async ValueTask<Unit> Handle(
        DeletePlayableCharacterCommand request,
        CancellationToken cancellationToken
    )
    {
        var unitEntity = await applicationDbContext
            .Units.Include(unit => unit.PlayableCharacter)
            .FirstOrDefaultAsync(
                unit => unit.GameUnitId == request.UnitId,
                cancellationToken: cancellationToken
            );

        Guard.Against.NotFound(request.UnitId, unitEntity);

        unitEntity.PlayableCharacter = null;
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
