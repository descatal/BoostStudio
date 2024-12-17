using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Series;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Series.Queries;

public record GetPlayableSeriesWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    string[]? Search = null
) : IRequest<PaginatedList<PlayableSeriesDto>>;

public class GetPlayableSeriesWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetPlayableSeriesWithPaginationQuery, PaginatedList<PlayableSeriesDto>>
{
    public async ValueTask<PaginatedList<PlayableSeriesDto>> Handle(
        GetPlayableSeriesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = applicationDbContext.PlayableSeries.AsQueryable();

        var mappedQueryable = PlayableSeriesMapper.ProjectToDto(query);
        var result = await PaginatedList<PlayableSeriesDto>.CreateAsync(mappedQueryable, request.Page, request.PerPage);

        return result;
    }
}
