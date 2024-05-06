using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs.UnitStats.Commands.Serializations;

public record SerializeUnitStatCommand(string DestinationPath) : IRequest;

public class SerializeStatCommandHandler(
    IApplicationDbContext applicationDbContext,
    IFormatBinarySerializer<StatsBinaryFormat> unitStatBinarySerializer,
    ILogger<SerializeStatCommandHandler> logger
) : IRequestHandler<SerializeUnitStatCommand>
{
    public async Task Handle(SerializeUnitStatCommand request, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.UnitStats.FirstOrDefaultAsync(cancellationToken);
        // var serializedBinary = await unitStatBinarySerializer.SerializeAsync(entity, cancellationToken);
        // await File.WriteAllBytesAsync(request.DestinationPath, serializedBinary, cancellationToken: cancellationToken);
    }
}
