﻿using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Queries.HitboxGroup;

public record GetHitboxGroupByHashQuery(uint Hash) : IRequest<HitboxGroupDto>;

public record GetHitboxGroupByUnitIdQuery(uint UnitId) : IRequest<HitboxGroupDto>;

public class GetHitboxGroupQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetHitboxGroupByHashQuery, HitboxGroupDto>, IRequestHandler<GetHitboxGroupByUnitIdQuery, HitboxGroupDto>
{
    public async Task<HitboxGroupDto> Handle(GetHitboxGroupByUnitIdQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.HitboxGroups
            .Where(group => request.UnitId == group.GameUnitId);
        
        var mappedQueryable = HitboxGroupMapper.ProjectToDto(query);
        var result = await mappedQueryable.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.UnitId, result);
        
        return result;
    }
    
    public async Task<HitboxGroupDto> Handle(GetHitboxGroupByHashQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.HitboxGroups
            .Where(group => request.Hash == group.Hash);
        
        var mappedQueryable = HitboxGroupMapper.ProjectToDto(query);
        var result = await mappedQueryable.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.Hash, result);
        
        return result;
    }
}
