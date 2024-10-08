﻿using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Common.Utils;

public static class PathUtils
{
    public static (string SourceDirectory, string DestinationPath) ParseSourceDirectory(
        string sourcePath,
        string destinationPath,
        string? fileName = null,
        ILogger? logger = null)
    {
        var outputFileName = fileName;
        
        if (string.IsNullOrWhiteSpace(outputFileName))
        {
            outputFileName = Path.GetFileNameWithoutExtension(sourcePath);
            logger?.LogInformation("No filename provided, using filename from source path: {outputFileName}", outputFileName);
        }
            
        var workingDirectory = Path.GetDirectoryName(sourcePath);
        
        // This is to support if the user does not supply any source path, we imply it as trying to use the current working directory as the source input path.
        if (string.IsNullOrWhiteSpace(workingDirectory))
        {
            workingDirectory = Directory.GetCurrentDirectory();
            logger?.LogInformation("Failed to parse working directory from source, using current directory as working directory: {workingDirectory}", workingDirectory);
        }

        var sourceDirectory = Path.Combine(workingDirectory, Path.GetFileName(sourcePath));

        var outputDirectory = destinationPath;
        if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(destinationPath)))
        {
            outputDirectory = Directory.GetCurrentDirectory();
            logger?.LogInformation("Failed to parse destination path, using the current directory as the outputDirectory: {outputDirectory}", outputDirectory);
        }
        
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
        
        destinationPath = Path.Combine(outputDirectory, outputFileName);

        logger?.LogInformation("Source directory is: {sourceDirectory}", sourceDirectory);
        logger?.LogInformation("Destination path is: {destinationPath}", destinationPath);
        
        return (sourceDirectory, destinationPath);
    }
}
