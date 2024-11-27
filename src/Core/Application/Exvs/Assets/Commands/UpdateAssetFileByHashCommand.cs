using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Assets;
using BoostStudio.Domain.Entities.Exvs.Assets;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Assets.Commands;

public record UpdateAssetFileByHashCommand(uint Hash) : AssetFileDto, IRequest;

public class UpdatePatchFileCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateAssetFileByHashCommand>
{
    public async ValueTask<Unit> Handle(UpdateAssetFileByHashCommand command, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.AssetFiles
            .FirstOrDefaultAsync(entity => entity.Hash == command.Hash, cancellationToken);

        Guard.Against.NotFound<AssetFile>(command.Hash.ToString(), entity);
        AssetFileMapper.Update(command, entity);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
