using BoostStudio.Application.Common.Enums;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using BoostStudio.Application.Common.Utils;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs2.Audio.Commands.Nus3Bank;

public record PackNus3BankPathCommand(
    string SourcePath, 
    string DestinationPath,
    string? FileName = null
) : IRequest;

public class PackNus3BankPathCommandHandler(
    IAudioConverter audioConverter,
    INus3Audio nus3Audio,
    ILogger<PackNus3BankPathCommandHandler> logger
) : IRequestHandler<PackNus3BankPathCommand>
{
    public async Task Handle(PackNus3BankPathCommand request, CancellationToken cancellationToken)
    {
        (string? sourceDirectory, string? destinationPath) = PathUtils.ParseSourceDirectory(
            request.SourcePath, 
            request.DestinationPath, 
            request.FileName);
        
        var temporaryWorkingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(temporaryWorkingDirectory);
        
        try
        {
            var filePaths = Directory.GetFiles(sourceDirectory, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                var subSongFileName = Path.GetFileNameWithoutExtension(filePath);
                var subSongFilePath = Path.Combine(temporaryWorkingDirectory, subSongFileName);
                subSongFilePath = Path.ChangeExtension(subSongFilePath, "wav");
                
                var audioBinary = await File.ReadAllBytesAsync(filePath, cancellationToken);
                var bnsfBinary = await audioConverter.ConvertAsync(audioBinary, AudioFormat.Bnsf, cancellationToken);
                await File.WriteAllBytesAsync(subSongFilePath, bnsfBinary, cancellationToken);
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
