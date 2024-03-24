using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record UnpackPsarcCommand(string SourceFilePath, string OutputDirectoryPath) : IRequest;

public class UnpackPsarcCommandHandler(IPsarcPacker psarcPacker) : IRequestHandler<UnpackPsarcCommand>
{
    public async Task Handle(UnpackPsarcCommand request, CancellationToken cancellationToken)
    {
        if (!File.Exists(request.SourceFilePath))
            throw new FileNotFoundException();

        var inputFileName = Path.GetFileNameWithoutExtension(request.SourceFilePath);
        var fallbackPath = Path.Combine(Path.GetDirectoryName(request.SourceFilePath) ?? Directory.GetCurrentDirectory(), inputFileName);
            
        var outputDirectory = string.IsNullOrWhiteSpace(request.SourceFilePath)
            ? fallbackPath
            : request.SourceFilePath;
        
        await psarcPacker.UnpackAsync(request.SourceFilePath, outputDirectory, cancellationToken);
        
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
    }
}
