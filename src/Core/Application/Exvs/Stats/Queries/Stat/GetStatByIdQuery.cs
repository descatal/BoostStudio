using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Queries.Stat;

public record GetStatByIdQuery(Guid Id) : IRequest<StatDto>;

public class GetStatByIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetStatByIdQuery, StatDto>
{
    public async Task<StatDto> Handle(GetStatByIdQuery request, CancellationToken cancellationToken)
    {
        var stats = applicationDbContext.Stats
            .Include(stat => stat.UnitStat)
            .Where(stat => stat.Id == request.Id);

        var queryableDto = StatMapper.ProjectToDto(stats);
        var dto = await queryableDto.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.Id, dto);
        
        return dto;
    }
}
