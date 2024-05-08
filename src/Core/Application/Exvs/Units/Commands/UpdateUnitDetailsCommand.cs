using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;
using Unit=BoostStudio.Domain.Entities.Unit.Unit;

namespace BoostStudio.Application.Exvs.Units.Commands;

public record UpdateUnitCommand(uint GameUnitId) : UnitDto, IRequest;

public class UpdateUnitCommandCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitCommand>
{
    public async Task Handle(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Units
            .FirstOrDefaultAsync(unit => unit.GameUnitId == command.GameUnitId, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.GameUnitId, existingEntity);
        
        await UpdateAsync(existingEntity, command, cancellationToken);
    }

    private async Task UpdateAsync(Unit existingEntity, UnitDto dto, CancellationToken cancellationToken = default)
    {
        existingEntity.Name = dto.Name; 
        existingEntity.NameJapanese = dto.NameJapanese; 
        existingEntity.NameChinese = dto.NameChinese; 
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
