namespace BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;

public interface IFhmPacker
{
    Task UnpackAsync(Fhm data, string outputDirectory, CancellationToken cancellationToken);

    Task<Fhm> PackAsync(Stream stream, string inputDirectory, CancellationToken cancellationToken);
}
