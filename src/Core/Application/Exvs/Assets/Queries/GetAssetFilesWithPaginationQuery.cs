using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Assets;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Exvs.Assets.Queries;

public record GetAssetFilesWithPagination(
    int Page = 1,
    int PerPage = 10,
    uint[]? UnitIds = null,
    AssetFileType[]? AssetFileTypes = null
) : IRequest<PaginatedList<AssetFileVm>>;

public class GetAssetFilesWithPaginationHandler(
    IApplicationDbContext applicationDbContext
): IRequestHandler<GetAssetFilesWithPagination, PaginatedList<AssetFileVm>>
{
    public async ValueTask<PaginatedList<AssetFileVm>> Handle(GetAssetFilesWithPagination request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.AssetFiles.AsQueryable();

        if (request.UnitIds?.Length > 0)
            query = query.Where(entity => entity.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId)));
        
        if (request.AssetFileTypes?.Length > 0)
        {
            query = query.Where(entity =>
                entity.FileType.Any(assetFileType => request.AssetFileTypes.Contains(assetFileType))
            );
        }

        var mappedQuery = AssetFileMapper.ProjectToVm(query);
        return await PaginatedList<AssetFileVm>.CreateAsync(mappedQuery, request.Page, request.PerPage);
    }
}
