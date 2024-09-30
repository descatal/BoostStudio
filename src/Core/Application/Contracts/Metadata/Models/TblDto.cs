namespace BoostStudio.Application.Contracts.Metadata.Models;

public record TblDto(uint CumulativeFileInfoCount, List<PatchFileMetadataDto> FileMetadata, List<string>? PathOrder = null);
