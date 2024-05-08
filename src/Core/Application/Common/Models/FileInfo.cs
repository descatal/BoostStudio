using System.Net.Mime;

namespace BoostStudio.Application.Common.Models;

public record FileInfo(
    byte[] Data,
    string FileName,
    string? MediaTypeName = null
);
