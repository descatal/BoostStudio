using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Entities.Exvs.Units.Characters;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Commands.PlayableCharacters;

public record UpsertPlayableCharacterCommand(uint UnitId) : PlayableCharacterDetailsDto, IRequest;

public class UpsertPlayableCharactersCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpsertPlayableCharacterCommand>
{
    public async ValueTask<Unit> Handle(UpsertPlayableCharacterCommand command, CancellationToken cancellationToken)
    {
        var unitEntity = await applicationDbContext.Units
            .Include(unit => unit.AssetFiles)
            .Include(unit => unit.PlayableCharacter)
            .FirstOrDefaultAsync(x => x.GameUnitId == command.UnitId, cancellationToken: cancellationToken);

        Guard.Against.NotFound(command.UnitId, unitEntity);

        var lastAssetFileOrder = await applicationDbContext.AssetFiles
            .MaxAsync(assetFile => assetFile.Order, cancellationToken: cancellationToken) + 1;

        var characterAssets = CharacterAssetUtils.GetAssetHashes(command);
        var allAssetHashes = characterAssets
            .Select(tuple => tuple.Item1)
            .ToList();

        var assetEntities = await applicationDbContext.AssetFiles
            .Where(assetFile => allAssetHashes.Contains(assetFile.Hash))
            .ToListAsync(cancellationToken);

        var target = unitEntity.PlayableCharacter ?? new PlayableCharacter
        {
            UnitId = command.UnitId
        };

        var source = PlayableCharacterMapper.MapToEntity(command.UnitId, command);
        PlayableCharacterMapper.UpdateEntity(source, target);
        unitEntity.PlayableCharacter = target;

        UnitMapper2.UpsertCharacterAssetFiles(applicationDbContext, lastAssetFileOrder, unitEntity, assetEntities, characterAssets);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}
