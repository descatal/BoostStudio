using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Assets;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Domain.Entities.Exvs.Assets;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Assets.Queries;

public record GetAssetFilesByHashQuery(uint Hash) : IRequest<AssetFileVm>;

public class GetPatchFilesByIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetAssetFilesByHashQuery, AssetFileVm>
{
    public async ValueTask<AssetFileVm> Handle(GetAssetFilesByHashQuery request, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext
            .AssetFiles
            .FirstOrDefaultAsync(patchFile => patchFile.Hash == request.Hash, cancellationToken);
        
        Guard.Against.NotFound<AssetFile>(request.Hash.ToString(), entity);
        
        return AssetFileMapper.ToVm(entity);
    }
}
