using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.PatchFiles.Queries;

public record GetPatchFilesByIdQuery(Guid Id) : IRequest<PatchFileVm>;

public class GetPatchFilesByIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetPatchFilesByIdQuery, PatchFileVm>
{
    public async ValueTask<PatchFileVm> Handle(GetPatchFilesByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext
            .PatchFiles
            .FirstOrDefaultAsync(patchFile => patchFile.Id == request.Id, cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);
        
        return PatchFilesMapper.ToVm(entity);
    }
}
