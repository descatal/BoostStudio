using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Units;
using Unit=BoostStudio.Domain.Entities.Unit.Unit;

namespace BoostStudio.Application.Exvs.Units.Commands;

public record CreateUnitCommand() : UnitDto, IRequest<Guid>;

public class CreateUnitCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateUnitCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateUnitCommand command, CancellationToken cancellationToken)
    {
        var entity = new Unit
        {
            GameUnitId = command.UnitId,
            Name = command.Name,
            NameJapanese = command.NameJapanese,
            NameChinese = command.NameChinese
        };
        
        applicationDbContext.Units.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
