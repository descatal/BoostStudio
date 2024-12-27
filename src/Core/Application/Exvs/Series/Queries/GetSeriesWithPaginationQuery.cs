using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Series;

namespace BoostStudio.Application.Exvs.Series.Queries;

public record GetSeriesWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    string[]? Search = null
) : IRequest<PaginatedList<SeriesDto>>;

public class GetSeriesWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetSeriesWithPaginationQuery, PaginatedList<SeriesDto>>
{
    public async ValueTask<PaginatedList<SeriesDto>> Handle(
        GetSeriesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Series.AsQueryable();

        var mappedQueryable = SeriesMapper.ProjectToDto(query);
        var result = await PaginatedList<SeriesDto>.CreateAsync(mappedQueryable, request.Page, request.PerPage);

        return result;
    }
}
