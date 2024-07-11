using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Units.Commands;

public record UpdateUnitCommand : UnitDto, IRequest;

public class UpdateUnitCommandCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitCommand>
{
    public async ValueTask<Unit> Handle(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Units
            .FirstOrDefaultAsync(unit => unit.GameUnitId == command.UnitId, cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(command.UnitId, existingEntity);
        
        existingEntity.Name = command.Name; 
        existingEntity.NameJapanese = command.NameJapanese; 
        existingEntity.NameChinese = command.NameChinese; 
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return default;
    }
}
