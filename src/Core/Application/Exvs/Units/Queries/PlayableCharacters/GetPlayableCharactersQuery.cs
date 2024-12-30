using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Queries.PlayableCharacters;

public record GetPlayableCharactersQuery(uint UnitId) : IRequest<PlayableCharacterDto>;

public class QueryPlayableCharactersHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetPlayableCharactersQuery, PlayableCharacterDto>
{
    public async ValueTask<PlayableCharacterDto> Handle(GetPlayableCharactersQuery command, CancellationToken cancellationToken)
    {
        var unit = await applicationDbContext.Units
            .Include(unit => unit.AssetFiles)
            .Include(unit => unit.PlayableCharacter)
            .FirstOrDefaultAsync(unit => unit.GameUnitId == command.UnitId, cancellationToken: cancellationToken);

        Guard.Against.NotFound(command.UnitId, unit);

        return UnitMapper2.MapToPlayableCharacterDto(unit);
    }
}
