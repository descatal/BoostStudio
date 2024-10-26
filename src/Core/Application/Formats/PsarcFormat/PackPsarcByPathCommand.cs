using System.ComponentModel;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Domain.Entities.PsarcFormat;
using Mediator;
using Microsoft.Extensions.Logging;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record PackPsarcByPathCommand : IRequest<FileInfo>
{
    public required string SourcePath { get; init; }

    public required string DestinationPath { get; init; }

    public string? Filename { get; init; }
    
    [DefaultValue(Domain.Entities.PsarcFormat.CompressionType.Zlib)]
    public CompressionType? CompressionType { get; init; }

    [DefaultValue(9)]
    public int? CompressionLevel { get; init; }
}

public class PackFhmCommandHandler(
    IPsarcPacker psarcPacker, 
    ILogger<PackFhmCommandHandler> logger
) : IRequestHandler<PackPsarcByPathCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(PackPsarcByPathCommand request, CancellationToken cancellationToken)
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

        if (!File.Exists(destinationPath))
            throw new Exception("Failed to create psarc archive.");
        
        var packedBytes = await File.ReadAllBytesAsync(destinationPath, cancellationToken);
        return new FileInfo(packedBytes, outputFileName);
    }
}
