using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Domain.Entities.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record PackPsarc : IRequest
{
    public string SourcePath { get; init; } = string.Empty;
    
    public string DestinationPath { get; init; } = string.Empty;

    public CompressionType CompressionType { get; init; } = CompressionType.None;

    public int CompressionLevel { get; init; } = 9;
}

public class PackFhmCommandHandler(IPsarcPacker psarcPacker) : IRequestHandler<PackPsarc>
{
    public async Task Handle(PackPsarc request, CancellationToken cancellationToken)
    {
        await psarcPacker.PackAsync(
            request.SourcePath, 
            request.DestinationPath,
            request.CompressionType, 
            request.CompressionLevel, 
            cancellationToken);
    }
}
