using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Formats.FhmFormat.Commands;

public record UnpackFhmPath(
    string SourceFilePath, 
    string OutputDirectoryPath,
    bool MultipleFiles = false
) : IRequest<FileInfo>;

public class UnpackFhmPathHandler(
    ISender sender,
    ILogger<UnpackFhmPathHandler> logger
) : IRequestHandler<UnpackFhmPath, FileInfo>
{
    public async ValueTask<FileInfo> Handle(UnpackFhmPath request, CancellationToken cancellationToken)
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
        
        return new FileInfo(inputBytes, inputFileName);
    }
}
