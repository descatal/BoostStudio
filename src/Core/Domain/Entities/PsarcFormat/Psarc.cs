namespace BoostStudio.Domain.Entities.PsarcFormat;

// TODO make this ksy compatible
public class Psarc
{
    public string SourcePath { get; set; } = string.Empty;

    public CompressionType CompressionType { get; set; } = CompressionType.Zlib;

    public int CompressionLevel { get; set; } = 9;
}

public enum CompressionType
{
    None,
    Zlib,
    Lzma,
}
