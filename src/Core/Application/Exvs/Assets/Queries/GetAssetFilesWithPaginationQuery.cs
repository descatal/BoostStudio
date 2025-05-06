using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Assets;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Assets.Queries;

public record GetAssetFilesWithPagination(
    int Page = 1,
    int PerPage = 10,
    uint[]? UnitIds = null,
    AssetFileType[]? AssetFileTypes = null,
    bool ListAll = false
) : IRequest<PaginatedList<AssetFileVm>>;

public class GetAssetFilesWithPaginationHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetAssetFilesWithPagination, PaginatedList<AssetFileVm>>
{
    public async ValueTask<PaginatedList<AssetFileVm>> Handle(
        GetAssetFilesWithPagination request,
        CancellationToken cancellationToken
    )
    {
        var query = applicationDbContext
            .AssetFiles.Include(assetFile => assetFile.Units)
            .AsQueryable();

        if (request.UnitIds?.Length > 0)
        {
            query = query.Where(entity =>
                entity.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId))
            );
        }

        if (request.AssetFileTypes?.Length > 0)
        {
            query = query.Where(entity =>
                entity.FileType.Any(assetFileType => request.AssetFileTypes.Contains(assetFileType))
            );
        }

        var mappedQuery = AssetFileMapper.ProjectToVm(query);

        if (!request.ListAll)
        {
            return await PaginatedList<AssetFileVm>.CreateAsync(
                mappedQuery,
                request.Page,
                request.PerPage
            );
        }

        var allData = await mappedQuery.ToListAsync(cancellationToken);
        return new PaginatedList<AssetFileVm>(allData, allData.Count, 1, allData.Count);
    }
}
