﻿using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Queries;

public record GetUnitsWithPaginationQuery(
    int Page = 1,
    int PerPage = 10
) : IRequest<PaginatedList<UnitDto2>>;

public class GetSeriesWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitsWithPaginationQuery, PaginatedList<UnitDto2>>
{
    public async ValueTask<PaginatedList<UnitDto2>> Handle(
        GetUnitsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Units
            .Include(unit => unit.PlayableCharacter)
            .AsQueryable();

        var mappedQueryable = UnitMapper2.ProjectToDto(query);
        var result = await PaginatedList<UnitDto2>.CreateAsync(mappedQueryable, request.Page, request.PerPage);

        return result;
    }
}