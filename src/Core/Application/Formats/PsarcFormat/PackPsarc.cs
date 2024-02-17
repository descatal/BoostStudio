using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Domain.Entities.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record PackPsarc : IRequest<byte[]>
{
    public string SourcePath { get; init; } = string.Empty;

    public CompressionType CompressionType { get; init; } = CompressionType.None;

    public int CompressionLevel { get; init; } = 9;
}

public class PackFhmCommandHandler(IPsarcPacker psarcPacker) : IRequestHandler<PackPsarc, byte[]>
{
    public async Task<byte[]> Handle(PackPsarc request, CancellationToken cancellationToken)
    {
        return await psarcPacker.PackAsync(
            request.SourcePath, 
            request.CompressionType, 
            request.CompressionLevel, 
            cancellationToken);
    }
}
