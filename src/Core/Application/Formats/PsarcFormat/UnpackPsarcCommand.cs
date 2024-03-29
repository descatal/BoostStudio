using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record UnpackPsarcCommand(string SourceFilePath, string OutputDirectoryPath) : IRequest;

public class UnpackPsarcCommandHandler(IPsarcPacker psarcPacker) : IRequestHandler<UnpackPsarcCommand>
{
    public async Task Handle(UnpackPsarcCommand request, CancellationToken cancellationToken)
    {
        if (!File.Exists(request.SourceFilePath))
            throw new FileNotFoundException();

        var input = request.SourceFilePath;
        var inputFileName = Path.GetFileNameWithoutExtension(input);
        var fallbackPath = Path.Combine(Path.GetDirectoryName(input) ?? Directory.GetCurrentDirectory(), inputFileName);
            
        var outputDirectory = string.IsNullOrWhiteSpace(input)
            ? fallbackPath
            : request.OutputDirectoryPath;
        
        await psarcPacker.UnpackAsync(input, outputDirectory, cancellationToken);
        
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
    }
}
