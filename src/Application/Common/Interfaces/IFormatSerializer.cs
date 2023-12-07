namespace BoostStudio.Application.Common.Interfaces;

public interface IFormatSerializer<T>
{
    Task<byte[]> SerializeAsync(T format, CancellationToken cancellationToken);

    Task<T> DeserializeAsync(byte[] data, CancellationToken cancellationToken);
    
    Task<T> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
