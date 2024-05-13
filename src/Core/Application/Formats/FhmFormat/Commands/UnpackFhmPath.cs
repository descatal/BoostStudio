using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Formats.FhmFormat.Commands;

public record UnpackFhmPath(
    string SourceFilePath, 
    string OutputDirectoryPath,
    bool MultipleFiles = false
) : IRequest;

public class UnpackFhmPathHandler(
    ISender sender,
    ILogger<UnpackFhmPathHandler> logger
) : IRequestHandler<UnpackFhmPath>
{
    public async Task Handle(UnpackFhmPath request, CancellationToken cancellationToken)
    {
        if (!File.Exists(request.SourceFilePath))
            throw new FileNotFoundException();

        var input = request.SourceFilePath;
        var inputFileName = Path.GetFileNameWithoutExtension(input);
        var fallbackPath = Path.Combine(Path.GetDirectoryName(input) ?? Directory.GetCurrentDirectory(), inputFileName);
            
        var outputDirectory = string.IsNullOrWhiteSpace(input)
            ? fallbackPath
            : request.OutputDirectoryPath;

        var inputBytes = await File.ReadAllBytesAsync(request.SourceFilePath, cancellationToken);
        await sender.Send(new UnpackFhmToDirectory(inputBytes, outputDirectory, request.MultipleFiles), cancellationToken);
    }
}
