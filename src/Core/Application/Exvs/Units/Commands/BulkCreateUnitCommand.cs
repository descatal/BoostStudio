using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using UnitEntity=BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Exvs.Units.Commands;

public record BulkCreateUnitCommand(UnitDto[] Units) : IRequest;

public class BulkCreateUnitCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<BulkCreateUnitCommand>
{
    public async ValueTask<Unit> Handle(BulkCreateUnitCommand command, CancellationToken cancellationToken)
    {
        var units = command.Units.OrderBy(unitDto => unitDto.UnitId).ToList();
        foreach (var unit in units)
        {
            var entity = new UnitEntity
            {
                GameUnitId = unit.UnitId,
                Name = unit.Name,
                NameJapanese = unit.NameJapanese,
                NameChinese = unit.NameChinese
            };

            applicationDbContext.Units.Add(entity);
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}
