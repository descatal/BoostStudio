namespace BoostStudio.Application.Contracts.Metadata.Models;

public record TblFileInfoMetadata(
    uint CumulativeIndex, 
    uint PatchNumber, 
    uint Size1, 
    uint Size2, 
    uint Size3, 
    uint Size4, 
    uint HashName);
