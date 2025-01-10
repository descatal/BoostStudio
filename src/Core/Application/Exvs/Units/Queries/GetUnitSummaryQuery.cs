using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Queries;

public record GetUnitSummaryQuery(
    string? Search = null,
    uint[]? UnitIds = null,
    LanguageSettings[]? Languages = null
) : IRequest<List<UnitSummaryVm>>;

public class GetUnitQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitSummaryQuery, List<UnitSummaryVm>>
{
    public async ValueTask<List<UnitSummaryVm>> Handle(GetUnitSummaryQuery request, CancellationToken cancellationToken)
    {
        var unitsQuery = applicationDbContext.Units
            .Include(unit => unit.PlayableCharacter)
            .AsQueryable();

        if (request.UnitIds is not null && request.UnitIds.Length != 0)
            unitsQuery = unitsQuery.Where(unit => request.UnitIds.Contains(unit.GameUnitId));

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            unitsQuery = unitsQuery.Where(unit =>
                unit.NameEnglish.ToLower().Contains(request.Search) ||
                unit.NameChinese.ToLower().Contains(request.Search) ||
                unit.NameJapanese.ToLower().Contains(request.Search));
        }
        
        var unit = await unitsQuery.ToListAsync(cancellationToken);
        return UnitMapper2.MapToVm(unit, request.Languages);
    }
}
