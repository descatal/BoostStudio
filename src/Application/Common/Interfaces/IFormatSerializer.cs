namespace BoostStudio.Application.Common.Interfaces;

public interface IFormatSerializer<T>
{
    Task<byte[]> SerializeAsync(T data, CancellationToken cancellationToken);

    Task<T> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
