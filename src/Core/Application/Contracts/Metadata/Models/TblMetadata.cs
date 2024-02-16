namespace BoostStudio.Application.Contracts.Metadata.Models;

public record TblMetadata(uint CumulativeFileInfoCount, List<TblFileMetadata> FileMetadata, List<string>? PathOrder = null);
