namespace BoostStudio.Application.Common.Interfaces;

public interface IFormatBinarySerializer<T>
{
    Task<byte[]> SerializeAsync(T data, CancellationToken cancellationToken);

    Task<T> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
