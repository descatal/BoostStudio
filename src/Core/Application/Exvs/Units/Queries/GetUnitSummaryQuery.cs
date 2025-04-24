using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Queries;

public enum GetUnitSummaryQueryIncludes
{
    Series = 0,
}

public record GetUnitSummaryQuery(
    int Page = 1,
    int PerPage = 10,
    bool ListAll = false,
    string? Search = null,
    uint[]? UnitIds = null,
    LanguageSettings[]? Languages = null,
    GetUnitSummaryQueryIncludes[]? Includes = null
) : IRequest<PaginatedList<UnitSummaryVm>>;

public class GetUnitQueryHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetUnitSummaryQuery, PaginatedList<UnitSummaryVm>>
{
    public async ValueTask<PaginatedList<UnitSummaryVm>> Handle(
        GetUnitSummaryQuery request,
        CancellationToken cancellationToken
    )
    {
        var unitsQuery = applicationDbContext
            .Units.Include(unit => unit.PlayableCharacter)
            .AsQueryable();

        if (request.Includes?.Length > 0)
        {
            unitsQuery = request.Includes.Aggregate(
                unitsQuery,
                (current, include) =>
                    include switch
                    {
                        GetUnitSummaryQueryIncludes.Series => current
                            .Include(unit => unit.PlayableCharacter)
                            .ThenInclude(playableCharacter => playableCharacter!.Series),
                        _ => current,
                    }
            );
        }

        if (request.UnitIds is not null && request.UnitIds.Length != 0)
            unitsQuery = unitsQuery.Where(unit => request.UnitIds.Contains(unit.GameUnitId));

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            unitsQuery = unitsQuery.Where(unit =>
                unit.NameEnglish.ToLower().Contains(request.Search)
                || unit.NameChinese.ToLower().Contains(request.Search)
                || unit.NameJapanese.ToLower().Contains(request.Search)
            );
        }

        var mappedQueryable = UnitMapper2.ProjectToVm(unitsQuery);

        if (!request.ListAll)
        {
            return await PaginatedList<UnitSummaryVm>.CreateAsync(
                mappedQueryable,
                request.Page,
                request.PerPage
            );
        }

        var allUnits = await mappedQueryable.ToListAsync(cancellationToken);
        return new PaginatedList<UnitSummaryVm>(allUnits, allUnits.Count, 1, allUnits.Count);
    }
}
