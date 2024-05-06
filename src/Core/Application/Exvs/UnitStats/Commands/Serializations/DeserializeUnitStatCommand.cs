using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;
using BoostStudio.Formats;

namespace BoostStudio.Application.Exvs.UnitStats.Commands.Serializations;

public record DeserializeUnitStatCommand(string SourceFilePath) : IRequest<StatsView>;

public class DeserializeUnitStatCommandHandler(
    IFormatBinarySerializer<StatsBinaryFormat> statBinarySerializer
) : IRequestHandler<DeserializeUnitStatCommand, StatsView>
{
    public async Task<StatsView> Handle(DeserializeUnitStatCommand request, CancellationToken cancellationToken)
    {
        var fileContent = await File.ReadAllBytesAsync(request.SourceFilePath, cancellationToken);

        await using var fileStream = new MemoryStream(fileContent);
        var deserializedStat = await statBinarySerializer.DeserializeAsync(fileStream, cancellationToken);
        
        return new StatsView(default);
    }
}
