using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Exvs.PatchFiles.Queries;

public record GetPatchFilesWithPagination(
    int Page = 1,
    int PerPage = 10,
    string? Search = null,
    PatchFileVersion[]? Versions = null,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<PatchFileVm>>;

public class GetPatchFilesWithPaginationHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetPatchFilesWithPagination, PaginatedList<PatchFileVm>>
{
    public async ValueTask<PaginatedList<PatchFileVm>> Handle(
        GetPatchFilesWithPagination request,
        CancellationToken cancellationToken
    )
    {
        var query = applicationDbContext.PatchFiles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var normalizedSearch = request.Search.ToLower();

            query = query.Where(entity =>
                (
                    entity.AssetFile != null
                    && (normalizedSearch.Contains(entity.AssetFile.Hash.ToString().ToLower()))
                )
                || (
                    entity.PathInfo != null
                    && request.Search.Contains(entity.PathInfo.Path.ToString())
                )
            );
        }

        if (request.UnitIds?.Length > 0)
        {
            query = query.Where(entity =>
                entity.AssetFile != null
                && entity.AssetFile.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId))
            );
        }

        if (request.Versions?.Length > 0)
            query = query.Where(entity => request.Versions.Contains(entity.TblId));

        var mappedQuery = PatchFilesMapper.ProjectToVm(query);
        return await PaginatedList<PatchFileVm>.CreateAsync(
            mappedQuery,
            request.Page,
            request.PerPage
        );
    }
}
