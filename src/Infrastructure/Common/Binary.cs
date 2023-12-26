namespace BoostStudio.Infrastructure.Common;

public static class Binary
{
    public static uint CalculateAlignment(uint length, uint alignment)
    {
        var remainder = length % alignment;
        if (remainder == 0)
            return length;

        var padding = alignment - remainder;
        return length + padding;
    }
    
    public static long CalculateAlignment(long length, uint alignment)
    {
        var remainder = length % alignment;
        if (remainder == 0)
            return length;

        var padding = alignment - remainder;
        return length + padding;
    }

    /// <summary>
    /// Pad byte array to specified alignment size
    /// </summary>
    /// <param name="array">Array to be aligned</param>
    /// <param name="alignment">Alignment size</param>
    /// <returns>Returns a copy of aligned byte array</returns>
    /// <exception cref="ArgumentException">Alignment must be a positive integer</exception>
    public static byte[] AlignByteArray(byte[] array, uint alignment)
    {
        if (alignment <= 0)
            throw new ArgumentException("Alignment must be a positive integer.");

        var remainder = (uint)array.Length % alignment;
        if (remainder == 0)
            return array;

        var padding = alignment - remainder;
        var paddedArray = new byte[array.Length + padding];
        Array.Copy(array, paddedArray, array.Length);

        return paddedArray;
    }
}
