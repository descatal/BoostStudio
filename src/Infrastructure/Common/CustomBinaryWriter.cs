using System;
using System.Buffers.Binary;
using System.Linq;
using System.Text;

namespace BoostStudio.Infrastructure.Common;

public enum Endianness
{
    LittleEndian,
    BigEndian
}

public class CustomBinaryWriter : BinaryWriter
{
    private readonly Dictionary<Type, Action<byte[], object>> _littleEndianMap = new()
    {
        {
            typeof(ushort), (bytes, value) => BinaryPrimitives.WriteUInt16LittleEndian(bytes, (ushort)value)
        },
        {
            typeof(short), (bytes, value) => BinaryPrimitives.WriteInt16LittleEndian(bytes, (short)value)
        },
        {
            typeof(uint), (bytes, value) => BinaryPrimitives.WriteUInt32LittleEndian(bytes, (uint)value)
        },
        {
            typeof(int), (bytes, value) => BinaryPrimitives.WriteInt32LittleEndian(bytes, (int)value)
        },
        {
            typeof(ulong), (bytes, value) => BinaryPrimitives.WriteUInt64LittleEndian(bytes, (ulong)value)
        },
        {
            typeof(long), (bytes, value) => BinaryPrimitives.WriteInt64LittleEndian(bytes, (long)value)
        },
        {
            typeof(float), (bytes, value) => BinaryPrimitives.WriteSingleLittleEndian(bytes, (float)value)
        },
        {
            typeof(double), (bytes, value) => BinaryPrimitives.WriteDoubleLittleEndian(bytes, (double)value)
        },
    };

    private readonly Dictionary<Type, Action<byte[], object>> _bigEndianMap = new()
    {
        {
            typeof(ushort), (bytes, value) => BinaryPrimitives.WriteUInt16BigEndian(bytes, (ushort)value)
        },
        {
            typeof(short), (bytes, value) => BinaryPrimitives.WriteInt16BigEndian(bytes, (short)value)
        },
        {
            typeof(uint), (bytes, value) => BinaryPrimitives.WriteUInt32BigEndian(bytes, (uint)value)
        },
        {
            typeof(int), (bytes, value) => BinaryPrimitives.WriteInt32BigEndian(bytes, (int)value)
        },
        {
            typeof(ulong), (bytes, value) => BinaryPrimitives.WriteUInt64BigEndian(bytes, (ulong)value)
        },
        {
            typeof(long), (bytes, value) => BinaryPrimitives.WriteInt64BigEndian(bytes, (long)value)
        },
        {
            typeof(float), (bytes, value) => BinaryPrimitives.WriteSingleBigEndian(bytes, (float)value)
        },
        {
            typeof(double), (bytes, value) => BinaryPrimitives.WriteDoubleBigEndian(bytes, (double)value)
        },
    };

    private readonly Dictionary<Type, int> _typeSize = new()
    {
        {
            typeof(ushort), sizeof(ushort)
        },
        {
            typeof(short), sizeof(short)
        },
        {
            typeof(uint), sizeof(uint)
        },
        {
            typeof(int), sizeof(int)
        },
        {
            typeof(ulong), sizeof(ulong)
        },
        {
            typeof(long), sizeof(long)
        },
        {
            typeof(float), sizeof(float)
        },
        {
            typeof(double), sizeof(double)
        },
    };

    public Stream Stream { get; }

    private readonly Endianness _endianness;

    public CustomBinaryWriter(Stream stream, Endianness defaultEndian = Endianness.LittleEndian) : base(stream)
    {
        Stream = stream;
        _endianness = defaultEndian;
    }

    public long GetPosition()
    {
        return OutStream.Position;
    }

    public long GetLength()
    {
        return OutStream.Length;
    }

    // There is no endianess for byte and byte arrays, it writes what you passed in
    public void WriteByte(byte value, long? position = null)
    {
        WriteByteArray(new byte[1]
        {
            value
        }, position);
    }

    public void WriteByteArray(byte[] value, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(ushort), null, position, alignment);
    }

    public void WriteUshort(ushort value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(ushort), endian, position, alignment);
    }

    public void WriteShort(short value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(short), endian, position, alignment);
    }

    public void WriteUint(uint value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(uint), endian, position, alignment);
    }

    public void WriteInt(int value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(int), endian, position, alignment);
    }

    public void WriteUlong(ulong value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(ulong), endian, position, alignment);
    }

    public void WriteLong(long value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(long), endian, position, alignment);
    }

    public void WriteFloat(float value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(float), endian, position, alignment);
    }

    public void WriteDouble(double value, Endianness? endian = null, long? position = null, uint? alignment = null)
    {
        Write(value, sizeof(double), endian, position, alignment);
    }

    /// <summary>
    /// Write string
    /// </summary>
    /// <param name="value">The string value</param>
    /// <param name="encoding">Encoding type to encode the values, will use Encoding.Default if null is supplied</param>
    /// <param name="writeSize">Optional bool to prepend the binary with a uint string size info, default is true</param>
    /// <param name="appendDelimiter">Optional bool to append a control char \0 at the end of the string, default is true</param>
    /// <param name="sizeEndian">
    /// Optional endian param to write the size info, if writeSize is true, default is null (reuse the endian set on
    /// initialization)
    /// </param>
    /// <param name="position">Optional stream position to write the string on (including size and endChar)</param>
    /// <param name="alignment">Optional buffer alignment size</param>
    public void WriteString(
        string value,
        Encoding encoding,
        bool writeSize = true,
        bool appendDelimiter = true,
        Endianness? sizeEndian = null,
        long? position = null,
        uint? alignment = null)
    {
        encoding = encoding ?? Encoding.Default;

        var encodedString = encoding.GetBytes(value);
        var encodedLength = encodedString.Length;

        if (writeSize)
            WriteUint((uint)encodedString.Length, sizeEndian);

        if (appendDelimiter)
            encodedString = encodedString.Concat(new byte[]
            {
                0x00
            }).ToArray();

        WriteByteArray(encodedString, position: (position + 4), alignment: alignment);
    }

    public void AlignStream(int alignment)
    {
        long padding = alignment - (OutStream.Position % alignment);
        if (padding == alignment)
            return;

        var buffer = new byte[padding];
        OutStream.Write(buffer, 0, buffer.Length);
    }

    public async Task ConcatenateStreamAsync(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        Stream.Seek(0, SeekOrigin.End);
        await stream.CopyToAsync(Stream);
    }

    public byte[] ToByteArray()
    {
        if (Stream is MemoryStream memoryStream)
            return memoryStream.ToArray();

        using var binaryReader = new BinaryReader(Stream);
        return binaryReader.ReadBytes((int)Stream.Length);
    }

    private void Write<T>(
        T value,
        int? byteCount = null,
        Endianness? endian = null,
        long? position = null,
        uint? alignment = null)
    {
        if (value == null)
            return;

        var localEndian = endian ?? _endianness;
        var valueType = value.GetType();

        byteCount ??= _typeSize[valueType];

        byte[] buffer = new byte[byteCount.Value];

        if (valueType == typeof(byte[]))
        {
            buffer = (byte[])Convert.ChangeType(value, typeof(byte[]));
        }
        else
        {
            switch (localEndian)
            {
                case Endianness.LittleEndian:
                    _littleEndianMap[valueType]?.Invoke(buffer, value);
                    break;
                case Endianness.BigEndian:
                    _bigEndianMap[valueType]?.Invoke(buffer, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(endian), endian, null);
            }
        }

        if (alignment.HasValue)
            buffer = AlignByteArray(buffer, alignment.Value);

        var returnPosition = OutStream.Position;
        if (position.HasValue)
            OutStream.Seek(position.Value, SeekOrigin.Begin);

        OutStream.Write(buffer);

        if (position.HasValue)
            OutStream.Seek(returnPosition, SeekOrigin.Begin);
    }

    private byte[] AlignByteArray(byte[] array, uint alignment)
    {
        if (alignment <= 0)
            throw new ArgumentException("Alignment must be a positive integer.");

        var remainder = (uint)array.Length % alignment;
        if (remainder == 0)
            return array;

        var padding = alignment - remainder;
        byte[] paddedArray = new byte[array.Length + padding];
        Array.Copy(array, paddedArray, array.Length);

        return paddedArray;
    }
}
