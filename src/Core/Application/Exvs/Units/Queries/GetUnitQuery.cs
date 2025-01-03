﻿using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;
using UnitMapper=BoostStudio.Application.Contracts.Units.UnitMapper;

namespace BoostStudio.Application.Exvs.Units.Queries;

public record GetUnitQuery(
    string? Search = null,
    uint[]? UnitIds = null
) : IRequest<List<UnitDto>>;

public class GetUnitQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitQuery, List<UnitDto>>
{
    public async ValueTask<List<UnitDto>> Handle(GetUnitQuery query, CancellationToken cancellationToken)
    {
        var unitsQuery = applicationDbContext.Units.AsQueryable();

        if (query.UnitIds is not null && query.UnitIds.Length != 0)
            unitsQuery = unitsQuery.Where(unit => query.UnitIds.Contains(unit.GameUnitId));

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            unitsQuery = unitsQuery.Where(unit =>
                unit.NameEnglish.ToLower().Contains(query.Search) ||
                unit.NameChinese.ToLower().Contains(query.Search) ||
                unit.NameJapanese.ToLower().Contains(query.Search));
        }
        
        var unit = await unitsQuery.ToListAsync(cancellationToken);
        return UnitMapper.MapToDto(unit);
    }
}
