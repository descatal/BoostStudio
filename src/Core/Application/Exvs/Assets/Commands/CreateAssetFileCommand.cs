using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Assets;

namespace BoostStudio.Application.Exvs.Assets.Commands;

public record CreateAssetFileCommand() : AssetFileDto, IRequest;

public class CreatePatchFileCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateAssetFileCommand>
{
    public async ValueTask<Unit> Handle(CreateAssetFileCommand command, CancellationToken cancellationToken)
    {
        var entity = AssetFileMapper.ToEntity(command);
        
        await applicationDbContext.AssetFiles.AddAsync(entity, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
