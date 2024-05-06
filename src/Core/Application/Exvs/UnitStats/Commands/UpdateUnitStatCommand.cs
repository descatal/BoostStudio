using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.UnitStats.Commands;

public record UpdateUnitStatCommand(Guid Id, uint GameUnitId) : IRequest;

public class UpdateStatSetCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitStatCommand>
{
    public async Task Handle(UpdateUnitStatCommand command, CancellationToken cancellationToken)
    {
        var unitStat = await applicationDbContext.UnitStats.FirstOrDefaultAsync(unitStat => unitStat.Id == command.Id, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.Id, unitStat);

        unitStat.GameUnitId = command.GameUnitId;
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
