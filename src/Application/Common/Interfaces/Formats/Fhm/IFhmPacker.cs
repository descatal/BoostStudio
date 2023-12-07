namespace BoostStudio.Application.Common.Interfaces.Formats.Fhm;

public interface IFhmPacker
{
    Task UnpackAsync(Contracts.Fhm fhm, string outputDirectory, CancellationToken cancellationToken);
    
    Task<Contracts.Fhm> PackAsync(Stream stream, string inputDirectory, CancellationToken cancellationToken);
}
