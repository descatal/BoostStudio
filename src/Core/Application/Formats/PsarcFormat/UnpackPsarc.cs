using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record UnpackPsarc(string SourceFilePath, string OutputDirectoryPath) : IRequest;

public class UnpackPsarcCommandHandler(IPsarcPacker psarcPacker) : IRequestHandler<UnpackPsarc>
{
    public async Task Handle(UnpackPsarc request, CancellationToken cancellationToken)
    {
        await psarcPacker.UnpackAsync(request.SourceFilePath, request.OutputDirectoryPath, cancellationToken);
    }
}
