using BoostStudio.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Scex.Commands;

public record CompileScexByPathCommand(
    string SourcePath,
    string DestinationPath,
    string? FileName = null,
    bool HotReload = true
) : IRequest<FileInfo>;

public class CompileScexByPathCommandHandler(
    IScexCompiler compiler,
    ISender sender,
    ILogger<CompileScexByPathCommandHandler> logger
) : IRequestHandler<CompileScexByPathCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(
        CompileScexByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        var outputFileName = request.FileName;

        if (string.IsNullOrWhiteSpace(outputFileName))
        {
            outputFileName = Path.GetFileNameWithoutExtension(request.SourcePath);
            logger.LogInformation(
                "No filename provided, using filename from source path: {outputFileName}",
                outputFileName
            );
        }

        var workingDirectory = Path.GetDirectoryName(request.SourcePath);

        // This is to support if the user does not supply any source path, we imply it as trying to use the current working directory as the source input path.
        if (string.IsNullOrWhiteSpace(workingDirectory))
        {
            workingDirectory = Directory.GetCurrentDirectory();
            logger.LogInformation(
                "Failed to parse working directory from source, using current directory as working directory: {workingDirectory}",
                workingDirectory
            );
        }

        var sourceDirectory = Path.Combine(workingDirectory, Path.GetFileName(request.SourcePath));
        logger.LogInformation("Source directory is: {sourceDirectory}", sourceDirectory);

        var outputDirectory = request.DestinationPath;

        if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(request.DestinationPath)))
        {
            outputDirectory = Directory.GetCurrentDirectory();
            logger.LogInformation(
                "Failed to parse destination path, using the current directory as the outputDirectory: {outputDirectory}",
                outputDirectory
            );
        }

        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        var destinationPath = Path.Combine(outputDirectory, outputFileName);

        logger.LogInformation("Destination path is: {destinationPath}", destinationPath);

        await compiler.CompileAsync(sourceDirectory, destinationPath, cancellationToken);

        if (request.HotReload)
            await sender.Send(new HotReloadScex(destinationPath), cancellationToken);

        if (!File.Exists(destinationPath))
            throw new Exception("Failed to compile scex file!");

        var compiledBytes = await File.ReadAllBytesAsync(destinationPath, cancellationToken);
        return new FileInfo(compiledBytes, outputFileName);
    }
}
