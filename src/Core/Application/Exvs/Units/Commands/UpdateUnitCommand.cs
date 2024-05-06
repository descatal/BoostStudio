using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;
using Unit=BoostStudio.Domain.Entities.Unit.Unit;

namespace BoostStudio.Application.Exvs.Units.Commands;

// Handles both types of Id which are both valid in Db.
public record UpdateUnitCommand(Guid Id) : UnitDetailsDto, IRequest;

public record UpdateUnitByGameUnitIdCommand(uint GameUnitId) : UnitDetailsDto, IRequest;

public class UpdateUnitCommandCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitCommand>, IRequestHandler<UpdateUnitByGameUnitIdCommand>
{
    public async Task Handle(UpdateUnitByGameUnitIdCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Units
            .FirstOrDefaultAsync(unit => unit.GameUnitId == command.GameUnitId, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.GameUnitId, existingEntity);
        
        await UpdateAsync(existingEntity, command, cancellationToken);
    }
    
    public async Task Handle(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Units
            .FirstOrDefaultAsync(unit => unit.Id == command.Id, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.Id, existingEntity);

        await UpdateAsync(existingEntity, command, cancellationToken);
    }

    private async Task UpdateAsync(Unit existingEntity, UnitDetailsDto detailsDto, CancellationToken cancellationToken = default)
    {
        existingEntity.Name = detailsDto.Name; 
        existingEntity.NameJapanese = detailsDto.NameJapanese; 
        existingEntity.NameChinese = detailsDto.NameChinese; 
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
