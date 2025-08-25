using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;

namespace BoostStudio.Application.Exvs.Psarc.Commands;

public record UnpackPsarcByPathCommand(string SourceFilePath, string OutputDirectoryPath)
    : IRequest;

public class UnpackPsarcCommandHandler(IPsarcPacker psarcPacker)
    : IRequestHandler<UnpackPsarcByPathCommand>
{
    public async ValueTask<Unit> Handle(
        UnpackPsarcByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!File.Exists(request.SourceFilePath))
            throw new FileNotFoundException();

        var input = request.SourceFilePath;
        var inputFileName = Path.GetFileNameWithoutExtension(input);
        var fallbackPath = Path.Combine(
            Path.GetDirectoryName(input) ?? Directory.GetCurrentDirectory(),
            inputFileName
        );

        var outputDirectory = string.IsNullOrWhiteSpace(input)
            ? fallbackPath
            : request.OutputDirectoryPath;

        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        await psarcPacker.UnpackAsync(input, outputDirectory, cancellationToken);

        return default;
    }
}
