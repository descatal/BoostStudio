using BoostStudio.Application.Common.Enums;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs2.Audio.Commands;

public record PackNus3AudioPathCommand(
    string SourcePath, 
    string DestinationPath,
    string? FileName = null) : IRequest;

public class PackBnsfPathCommandHandler(
    IAudioConverter audioConverter,
    INus3Audio nus3Audio,
    ILogger<PackBnsfPathCommandHandler> logger
) : IRequestHandler<PackNus3AudioPathCommand>
{
    public async Task Handle(PackNus3AudioPathCommand request, CancellationToken cancellationToken)
    {
        var outputFileName = request.FileName;
        
        if (string.IsNullOrWhiteSpace(outputFileName))
        {
            outputFileName = Path.GetFileNameWithoutExtension(request.SourcePath);
            logger.LogInformation("No filename provided, using filename from source path: {outputFileName}", outputFileName);
        }
            
        var workingDirectory = Path.GetDirectoryName(request.SourcePath);
        
        // This is to support if the user does not supply any source path, we imply it as trying to use the current working directory as the source input path.
        if (string.IsNullOrWhiteSpace(workingDirectory))
        {
            workingDirectory = Directory.GetCurrentDirectory();
            logger.LogInformation("Failed to parse working directory from source, using current directory as working directory: {workingDirectory}", workingDirectory);
        }

        var sourceDirectory = Path.Combine(workingDirectory, Path.GetFileName(request.SourcePath));
        logger.LogInformation("Source directory is: {sourceDirectory}", sourceDirectory);

        var outputDirectory = request.DestinationPath;

        if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(request.DestinationPath)))
        {
            outputDirectory = Directory.GetCurrentDirectory();
            logger.LogInformation("Failed to parse destination path, using the current directory as the outputDirectory: {outputDirectory}", outputDirectory);
        }
            
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
        
        var destinationPath = Path.Combine(outputDirectory, outputFileName);
        
        logger.LogInformation("Destination path is: {destinationPath}", destinationPath);

        var temporaryWorkingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(temporaryWorkingDirectory);
        
        try
        {
            var filePaths = Directory.GetFiles(request.SourcePath, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var bnsfFilePath = Path.Combine(temporaryWorkingDirectory, fileName);
                bnsfFilePath = Path.ChangeExtension(bnsfFilePath, "bnsf");
                
                var audioBinary = await File.ReadAllBytesAsync(filePath, cancellationToken);
                var bnsfBinary = await audioConverter.ConvertAsync(audioBinary, AudioFormat.Bnsf, cancellationToken);
                await File.WriteAllBytesAsync(bnsfFilePath, bnsfBinary, cancellationToken);
            }

            var outputBinary = await nus3Audio.PackDirectoryToNus3AudioAsync(temporaryWorkingDirectory, cancellationToken);
            await File.WriteAllBytesAsync(destinationPath, outputBinary, cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(temporaryWorkingDirectory, true);
        }
    }
}
