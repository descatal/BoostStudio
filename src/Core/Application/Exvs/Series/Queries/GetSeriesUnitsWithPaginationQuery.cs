using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Series.Queries;

public record GetSeriesUnitsWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    bool ListAll = false
) : IRequest<PaginatedList<SeriesUnitsVm>>;

public class GetSeriesUnitsWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetSeriesUnitsWithPaginationQuery, PaginatedList<SeriesUnitsVm>>
{
    public async ValueTask<PaginatedList<SeriesUnitsVm>> Handle(GetSeriesUnitsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Units
            .Include(x => x.PlayableCharacter)
            .ThenInclude(x => x!.Series)
            .AsQueryable();

        var groupedQuery = query
            .Where(unit => unit.PlayableCharacter != null && unit.PlayableCharacter.Series != null)
            .GroupBy(unit => unit.PlayableCharacter!.Series)
            .Select(grouping => new
            {
                Meta = SeriesMapper.MapToSeriesUnitsVm(grouping.Key!), Units = UnitMapper2.MapToVm(grouping.ToList())
            })
            .Select(map => new SeriesUnitsVm()
            {
                Id = map.Meta.Id,
                NameChinese = map.Meta.NameChinese,
                NameEnglish = map.Meta.NameEnglish,
                NameJapanese = map.Meta.NameJapanese,
                SlugName = map.Meta.SlugName,
                Units = map.Units
            });

        if (!request.ListAll)
            return await PaginatedList<SeriesUnitsVm>.CreateAsync(groupedQuery, request.Page, request.PerPage);

        var items = await groupedQuery.ToListAsync(cancellationToken);
        return new PaginatedList<SeriesUnitsVm>(items, items.Count, 1, items.Count);
    }
}
