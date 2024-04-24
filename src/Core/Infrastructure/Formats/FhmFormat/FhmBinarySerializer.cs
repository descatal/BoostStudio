using System.Security.Cryptography;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Infrastructure.Common;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.FhmFormat;

public class FhmBinarySerializer : IFormatBinarySerializer<Fhm>
{
    public Task<Fhm> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new Fhm(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }

    public async Task<byte[]> SerializeAsync(Fhm data, CancellationToken cancellationToken)
    {
        if (data.Body?.FileContent is not Fhm.FhmBody fhmBody)
            return Array.Empty<byte>();

        return await SerializeFhmBodyAsync(fhmBody, cancellationToken);
    }

    private async Task<byte[]> SerializeFhmBodyAsync(Fhm.FhmBody fhmBody, CancellationToken cancellationToken)
    {
        await using var fhmMetadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        fhmMetadataStream.WriteUint(0x46484D20); // Magic
        fhmMetadataStream.WriteByteArray([0x1, 0x1]);
        fhmMetadataStream.WriteByteArray([0x0, 0x10]);
        fhmMetadataStream.WriteUint(0);

        var sizePosition = fhmMetadataStream.GetPosition();
        fhmMetadataStream.WriteUint(0);
        fhmMetadataStream.WriteUint((uint)fhmBody.Files.Count);

        // Precalculate size of whole fhm (with 0x10 padding) as the currentOffset
        var fhmSize = (uint)fhmMetadataStream.Stream.Length + (uint)(fhmBody.Files.Count * 4 * 4);
        var currentOffset = Binary.CalculateAlignment(fhmSize, 0x10);

        await using var offsetStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var sizeStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var loadTypeStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var unkTypeStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var fileBodyStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        var checksumOffsetMap = new Dictionary<string, long>();
        foreach (var fileBody in fhmBody.Files)
        {
            byte[] fileData;

            switch (fileBody.Body?.FileContent)
            {
                // Recursively serialize files for nested Fhm
                case Fhm.FhmBody nestedFhmBody:
                    {
                        fileData = await SerializeFhmBodyAsync(nestedFhmBody, cancellationToken);
                        fileBodyStream.WriteByteArray(fileData);
                        break;
                    }
                case Fhm.GenericBody genericBody:
                    {
                        var fileMagic = BitConverter.GetBytes((uint)fileBody.Body.FileMagic);
                        Array.Reverse(fileMagic);
                        fileData = [..fileMagic, ..genericBody.Body];
                        break;
                    }
                default:
                    continue;
            }

            var originalFileSize = (uint)fileData.Length;

            // Align the file data to 0x10
            fileData = Binary.AlignByteArray(fileData, 0x10);
            var alignedFileSize = (uint)fileData.Length;

            offsetStream.WriteUint(currentOffset);
            sizeStream.WriteUint(originalFileSize);
            loadTypeStream.WriteUint((uint)fileBody.AssetLoadType);
            unkTypeStream.WriteUint((uint)fileBody.UnkType);

            // Calculate the next offset, based on the aligned file size
            // Fhm files allows us to specify the same pointer for identical files, which can save space while packing
            var md5Hash = Convert.ToHexString(MD5.HashData(fileData));
            if (!checksumOffsetMap.ContainsKey(md5Hash))
                currentOffset += alignedFileSize;
            checksumOffsetMap.Add(md5Hash, currentOffset);

            fileBodyStream.WriteByteArray(fileData);
        }

        // Concatenate all file metadata streams, then align to 0x10
        await fhmMetadataStream.ConcatenateStreamAsync(offsetStream.Stream);
        await fhmMetadataStream.ConcatenateStreamAsync(sizeStream.Stream);
        await fhmMetadataStream.ConcatenateStreamAsync(loadTypeStream.Stream);
        await fhmMetadataStream.ConcatenateStreamAsync(unkTypeStream.Stream);
        fhmMetadataStream.AlignStream(0x10);

        // Concatenate the file metadata stream with the file body stream
        await using var fhmStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fhmStream.ConcatenateStreamAsync(fhmMetadataStream.Stream);
        await fhmStream.ConcatenateStreamAsync(fileBodyStream.Stream);

        // Go back and write back the size
        fhmStream.WriteUint((uint)fhmStream.GetLength(), position: sizePosition);
        return fhmStream.ToByteArray();
    }
}
