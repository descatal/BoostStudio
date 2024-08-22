namespace BoostStudio.Application.Contracts.Metadata.Models;

public record TblDto(uint CumulativeFileInfoCount, List<PatchFileDto> FileMetadata, List<string>? PathOrder = null);
