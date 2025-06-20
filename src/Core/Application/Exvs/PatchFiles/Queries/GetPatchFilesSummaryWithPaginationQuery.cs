﻿using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.PatchFiles.Queries;

public record GetPatchFilesSummaryWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    PatchFileVersion[]? Versions = null,
    uint[]? UnitIds = null,
    uint[]? AssetFileHashes = null,
    AssetFileType[]? AssetFileTypes = null
) : IRequest<PaginatedList<PatchFileSummaryVm>>;

public class GetPatchFilesSummaryWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetPatchFilesSummaryWithPaginationQuery, PaginatedList<PatchFileSummaryVm>>
{
    public async ValueTask<PaginatedList<PatchFileSummaryVm>> Handle(
        GetPatchFilesSummaryWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = applicationDbContext
            .PatchFiles.Include(entity => entity.AssetFile)
            .ThenInclude(entity => entity!.Units)
            .AsQueryable();

        if (request.UnitIds?.Length > 0)
        {
            query = query.Where(entity =>
                entity.AssetFile != null
                && entity.AssetFile.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId))
            );
        }

        if (request.AssetFileHashes?.Length > 0)
            query = query.Where(entity =>
                request.AssetFileHashes.Any(hash => hash == entity.AssetFileHash)
            );

        if (request.AssetFileTypes?.Length > 0)
        {
            query = query.Where(entity =>
                entity.AssetFile != null
                && request.AssetFileTypes.Any(type => entity.AssetFile.FileType.Contains(type))
            );
        }

        if (request.Versions?.Length > 0)
            query = query.Where(entity => request.Versions.Contains(entity.TblId));

        query = query.OrderBy(patchFile =>
            patchFile.PathInfo == null ? 0 : patchFile.PathInfo.Order
        );

        var mappedQuery = PatchFilesMapper.ProjectToSummaryVm(query);
        return await PaginatedList<PatchFileSummaryVm>.CreateAsync(
            mappedQuery,
            request.Page,
            request.PerPage
        );
    }
}
