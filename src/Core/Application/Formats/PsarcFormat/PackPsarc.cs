using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record PackPsarc : IRequest<byte[]>
{
    public string SourcePath { get; init; } = string.Empty;

    public CompressionType CompressionType { get; init; } = CompressionType.None;

    public int CompressionLevel { get; init; } = 9;
}

public class PackFhmCommandHandler(IFormatSerializer<Psarc> formatSerializer) : IRequestHandler<PackPsarc, byte[]>
{
    public async Task<byte[]> Handle(PackPsarc request, CancellationToken cancellationToken)
    {
        var psarc = new Psarc
        {
            SourcePath = request.SourcePath,
            CompressionType = request.CompressionType,
            CompressionLevel = request.CompressionLevel
        };
        return await formatSerializer.SerializeAsync(psarc, cancellationToken);
    }
}

