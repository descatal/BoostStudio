using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Debugging;

public record DebugCommand : IRequest;

public class DebugCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<DebugCommand>
{
    public async ValueTask<Unit> Handle(DebugCommand request, CancellationToken cancellationToken)
    {
        var foo = await applicationDbContext.AssetFiles.ToListAsync(cancellationToken);

        foreach (var bar in foo)
        {
            var fileTypes = bar.FileType.Distinct();
            bar.FileType = fileTypes.ToList();
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }
}
