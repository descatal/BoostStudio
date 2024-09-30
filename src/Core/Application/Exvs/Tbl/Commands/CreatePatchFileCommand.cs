using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;

namespace BoostStudio.Application.Exvs.Tbl.Commands;

public record CreatePatchFileCommand() : PatchFileDto, IRequest;

public class CreatePatchFileCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreatePatchFileCommand>
{
    public async ValueTask<Unit> Handle(CreatePatchFileCommand command, CancellationToken cancellationToken)
    {
        var entity = PatchFilesMapper.ToEntity(command);
        
        await applicationDbContext.PatchFiles.AddAsync(entity, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
