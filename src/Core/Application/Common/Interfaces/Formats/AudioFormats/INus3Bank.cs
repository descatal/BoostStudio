namespace BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

public interface INus3Bank
{
    Task<byte[]> PackDirectoryToNus3BankAsync(string sourcePath, CancellationToken cancellationToken = default);

    Task UnpackNus3BankAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);
}
