﻿using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;
using UnitMapper=BoostStudio.Application.Contracts.Mappers.UnitMapper;

namespace BoostStudio.Application.Exvs.Units.Queries;

public record GetUnitQueryByUnitId(uint UnitId) : IRequest<UnitDto>;

public class GetUnitQueryByUnitIdHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitQueryByUnitId, UnitDto>
{
    public async ValueTask<UnitDto> Handle(GetUnitQueryByUnitId query, CancellationToken cancellationToken)
    {
        var unitsQuery = applicationDbContext.Units.AsQueryable();
        var entity = await unitsQuery
            .FirstOrDefaultAsync(unit => query.UnitId == unit.GameUnitId, cancellationToken: cancellationToken);

        Guard.Against.NotFound(query.UnitId, entity);
        return UnitMapper.MapToDto(entity);
    }
}