namespace BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

public interface INus3Audio
{
    Task<byte[]> PackDirectoryToNus3AudioAsync(
        string sourcePath, 
        CancellationToken cancellationToken = default);

    Task UnpackNus3AudioAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);
}
