using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using Microsoft.Extensions.Logging;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Fhm.Commands;

public record PackFhmByPathCommand(
    string SourcePath,
    string DestinationPath,
    string? FileName = null
) : IRequest<FileInfo>;

public class PackFhmByPathCommandHandler(
    IFormatBinarySerializer<BoostStudio.Formats.Fhm> formatBinarySerializer,
    IFhmPacker fhmPacker,
    ILogger<PackFhmByPathCommandHandler> logger
) : IRequestHandler<PackFhmByPathCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(
        PackFhmByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        var outputFileName = request.FileName;

        if (string.IsNullOrWhiteSpace(outputFileName))
        {
            outputFileName = Path.GetFileNameWithoutExtension(request.SourcePath);
            logger.LogInformation(
                "No filename provided, using filename from source path: {OutputFileName}",
                outputFileName
            );
        }

        var workingDirectory = Path.GetDirectoryName(request.SourcePath);

        // This is to support if the user does not supply any source path, we imply it as trying to use the current working directory as the source input path.
        if (string.IsNullOrWhiteSpace(workingDirectory))
        {
            workingDirectory = Directory.GetCurrentDirectory();
            logger.LogInformation(
                "Failed to parse working directory from source, using current directory as working directory: {WorkingDirectory}",
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
                "Failed to parse destination path, using the current directory as the outputDirectory: {OutputDirectory}",
                outputDirectory
            );
        }

        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        var destinationPath = Path.Combine(outputDirectory, outputFileName);

        logger.LogInformation("Destination path is: {DestinationPath}", destinationPath);

        using var stream = new MemoryStream();
        var packedFhm = await fhmPacker.PackAsync(stream, sourceDirectory, cancellationToken);
        var serializedFhm = await formatBinarySerializer.SerializeAsync(
            packedFhm,
            cancellationToken
        );
        await File.WriteAllBytesAsync(destinationPath, serializedFhm, cancellationToken);

        return new FileInfo(serializedFhm, outputFileName);
    }
}
