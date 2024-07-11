using BoostStudio.Application.Common.Enums;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

namespace BoostStudio.Application.Exvs2.Audio.Commands.Nus3Audio;

public record UnpackNus3AudioPathCommand(
    string SourceFilePath,
    string OutputDirectoryPath
) : IRequest;

public class UnpackNus3AudioPathCommandHandler(
    INus3Audio nus3Audio,
    IAudioConverter audioConverter
) : IRequestHandler<UnpackNus3AudioPathCommand>
{
    public async ValueTask<Unit> Handle(UnpackNus3AudioPathCommand request, CancellationToken cancellationToken)
    {
        if (!File.Exists(request.SourceFilePath))
            throw new FileNotFoundException();

        var input = request.SourceFilePath;
        var inputFileName = Path.GetFileNameWithoutExtension(input);
        var fallbackPath = Path.Combine(Path.GetDirectoryName(input) ?? Directory.GetCurrentDirectory(), inputFileName);
            
        var outputDirectory = string.IsNullOrWhiteSpace(input)
            ? fallbackPath
            : request.OutputDirectoryPath;

        await nus3Audio.UnpackNus3AudioAsync(request.SourceFilePath, outputDirectory, cancellationToken: cancellationToken);
        
        // Rename known formats
        var filePaths = Directory.GetFiles(outputDirectory, "*", SearchOption.TopDirectoryOnly);
        foreach (var filePath in filePaths)
        {
            await using var audioFileStream = File.OpenRead(filePath);
            var audioFormat = audioConverter.GetAudioFormat(audioFileStream);
            audioFileStream.Close();
            
            switch (audioFormat)
            {
                case AudioFormat.Bnsf:
                    File.Move(filePath, Path.ChangeExtension(filePath, "bnsf"), true);
                    break;
                case AudioFormat.Riff:
                    File.Move(filePath, Path.ChangeExtension(filePath, "wav"), true);
                    break;
            }
        }

        return default;
    }
}
