using System.ComponentModel;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Application.Formats.FhmFormat.Commands;
using BoostStudio.Domain.Entities.PsarcFormat;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record PackPsarcCommand : IRequest
{
    public required string SourcePath { get; init; }

    public required string DestinationPath { get; init; }

    public string? Filename { get; init; }
    
    [DefaultValue(Domain.Entities.PsarcFormat.CompressionType.Zlib)]
    public CompressionType? CompressionType { get; init; }

    [DefaultValue(9)]
    public int? CompressionLevel { get; init; }
}

public class PackFhmCommandHandler(IPsarcPacker psarcPacker, ILogger<PackFhmCommandHandler> logger) : IRequestHandler<PackPsarcCommand>
{
    public async Task Handle(PackPsarcCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received command to pack psarc {@Request}", request);
        
        var compressionType = request.CompressionType ?? CompressionType.Zlib;
        var compressionLevel = request.CompressionLevel ?? 9;

        var outputFileName = request.Filename;
        
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
        
        var destinationPath = Path.Combine(outputDirectory, Path.ChangeExtension(outputFileName, ".psarc"));
        
        logger.LogInformation("Destination path is: {destinationPath}", destinationPath);
        
        await psarcPacker.PackAsync(
            sourceDirectory, 
            destinationPath,
            compressionType, 
            compressionLevel, 
            cancellationToken);
    }
}
