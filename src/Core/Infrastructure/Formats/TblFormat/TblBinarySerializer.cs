using System.Buffers.Binary;
using System.Collections;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.TblFormat;

public class TblBinarySerializer : ITblBinarySerializer
{
    public Task<TblBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new TblBinaryFormat((uint)data.Length, kaitaiStream);
        return Task.FromResult(deserializedObject);
    }

    public async Task<byte[]> SerializeAsync(TblBinaryFormat data, CancellationToken cancellationToken)
    {
        await using var tblMetadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        tblMetadataStream.WriteUint(0x54424C20); // Magic
        tblMetadataStream.WriteByteArray([0x1, 0x1]);
        tblMetadataStream.WriteByteArray([0x0, 0x0]);
        tblMetadataStream.WriteUint((uint)data.FilePaths.Count);
        tblMetadataStream.WriteUint((uint)data.CumulativeFileCount);

        var fileInfoPointer = Binary.CalculateAlignment(
            tblMetadataStream.GetLength() +
            (data.FilePaths.Count * 4) +
            (data.CumulativeFileCount * 4), 0x8);
        var filePathPointer = fileInfoPointer + (data.FileInfos.Count * 0x20);

        await using var fileInfoPointerStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var fileInfoStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var filePathPointerStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var filePathStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        for (var i = 0; i < data.CumulativeFileCount; i++)
        {
            var fileInfoBody = data.FileInfos.FirstOrDefault(fileInfo => fileInfo.Index.Equals(i));

            // Write pointer for file info.
            // If the file index does not exist in the metadata, write 0.
            fileInfoPointerStream.WriteUint(fileInfoBody is null ? 0u : (uint)(fileInfoPointer + fileInfoStream.GetLength()));

            if (fileInfoBody?.FileInfo is null)
                continue;

            var fileInfo = fileInfoBody.FileInfo;
            fileInfoStream.WriteUint(fileInfo.PatchNumber);
            fileInfoStream.WriteUint((uint)fileInfo.PathIndex);
            fileInfoStream.WriteByteArray(new byte[]
            {
                0x0, 0x4, 0x0, 0x0
            });
            fileInfoStream.WriteUint(fileInfo.Size1);
            fileInfoStream.WriteUint(fileInfo.Size2);
            fileInfoStream.WriteUint(fileInfo.Size3);
            fileInfoStream.WriteUint(0u);
            fileInfoStream.WriteUint(fileInfo.HashName);
        }

        foreach (var filePathBody in data.FilePaths)
        {
            var filePath = filePathBody?.Path ?? string.Empty;
            // No idea how the subdirectory flag works, if it is within the first directory it seems like it is 0
            // Anything that has sub-sub directory has 0x8000 flag.
            var subdirectoryFlag = string.IsNullOrWhiteSpace(Path.GetDirectoryName(Path.GetDirectoryName(filePath))) ? 0x0 : 0x8000;

            filePathPointerStream.WriteUshort((ushort)subdirectoryFlag);
            filePathPointerStream.WriteUshort((ushort)(filePathPointer + filePathStream.GetLength()));
            filePathStream.WriteString(filePath, Encoding.Default, writeSize: false, appendDelimiter: true);
        }

        // Concatenate the file metadata stream with the file body stream
        await using var tblStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await tblStream.ConcatenateStreamAsync(tblMetadataStream.Stream);
        await tblStream.ConcatenateStreamAsync(filePathPointerStream.Stream);
        await tblStream.ConcatenateStreamAsync(fileInfoPointerStream.Stream);
        tblStream.AlignStream(0x8);
        await tblStream.ConcatenateStreamAsync(fileInfoStream.Stream);
        await tblStream.ConcatenateStreamAsync(filePathStream.Stream);

        return tblStream.ToByteArray();
    }

    public async Task<byte[]> SerializeAsync(Tbl data, CancellationToken cancellationToken)
    {
        var paths = data.PatchFiles
            .Where(patchFile => patchFile.PathInfo is not null)
            .OrderBy(patchFile => patchFile.PathInfo!.Order)
            .Select(patchFile => patchFile.PathInfo!.Path)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(path => path!)
            .ToList();
        
        await using var tblMetadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        tblMetadataStream.WriteUint(0x54424C20); // Magic
        tblMetadataStream.WriteByteArray([0x1, 0x1]);
        tblMetadataStream.WriteByteArray([0x0, 0x0]);
        tblMetadataStream.WriteUint((uint)paths.Count);
        tblMetadataStream.WriteUint(data.CumulativeAssetIndex);

        var fileInfoPointer = Binary.CalculateAlignment(
            tblMetadataStream.GetLength() +
            (paths.Count * 4) +
            (data.CumulativeAssetIndex * 4), 0x8);

        var fileInfoEntries = data.PatchFiles.Count(patchFile => (patchFile.FileInfo is not null || patchFile.AssetFileHash is not null));
        var filePathPointer = fileInfoPointer + (fileInfoEntries * 0x20);

        await using var fileInfoPointerStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var fileInfoStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var filePathPointerStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var filePathStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        for (var index = 0; index < data.CumulativeAssetIndex; index++)
        {
            var patchFile = data.PatchFiles.FirstOrDefault(patchFile =>
                    patchFile.FileInfo is not null &&
                    patchFile.AssetFile is not null &&
                    patchFile.AssetFile.Order == index + 1
                );

            // Write pointer for file info.
            // If the file index does not exist in the metadata, write 0.
            var currentFileInfoPointer = patchFile?.FileInfo is null
                ? 0u
                : (uint)(fileInfoPointer + fileInfoStream.GetLength());
            fileInfoPointerStream.WriteUint(currentFileInfoPointer);

            if (patchFile?.FileInfo is null || patchFile.AssetFile is null)
                continue;

            var filePathIndex = paths.IndexOf(patchFile.PathInfo?.Path ?? string.Empty);
            fileInfoStream.WriteUint((uint)patchFile.FileInfo.Version);
            fileInfoStream.WriteUint(filePathIndex == -1 ? 0u : (uint)filePathIndex);
            fileInfoStream.WriteByteArray([0x0, 0x4, 0x0, 0x0]);
            fileInfoStream.WriteUint((uint)patchFile.FileInfo.Size1);
            fileInfoStream.WriteUint((uint)patchFile.FileInfo.Size2);
            fileInfoStream.WriteUint((uint)patchFile.FileInfo.Size3);
            fileInfoStream.WriteUint((uint)patchFile.FileInfo.Size4);
            fileInfoStream.WriteUint(patchFile.AssetFile.Hash);
        }

        foreach (var filePath in paths)
        {
            // No idea how the subdirectory flag works, if it is within the first directory it seems like it is 0
            // Anything that has sub-sub directory has 0x80 flag.
            var subdirectoryFlag = string.IsNullOrWhiteSpace(Path.GetDirectoryName(Path.GetDirectoryName(filePath)))
                ? (byte)0x0
                : (byte)0x80;

            filePathPointerStream.WriteByte(subdirectoryFlag);
            
            // the size is a rare type of 3 byte integer
            var pointer = (uint)(filePathPointer + filePathStream.GetLength());
            var pointerByteArray = BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(pointer));
            // remove the first byte, hopefully the pointer of the file won't go over 0xFFFFFF, or around 16mb (shit load of information)
            filePathPointerStream.WriteByteArray([
                pointerByteArray[1], 
                pointerByteArray[2], 
                pointerByteArray[3]
            ]);
            
            filePathStream.WriteString(filePath, Encoding.Default, writeSize: false, appendDelimiter: true);
        }

        // Concatenate the file metadata stream with the file body stream
        await using var tblStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await tblStream.ConcatenateStreamAsync(tblMetadataStream.Stream);
        await tblStream.ConcatenateStreamAsync(filePathPointerStream.Stream);
        await tblStream.ConcatenateStreamAsync(fileInfoPointerStream.Stream);
        tblStream.AlignStream(0x8);
        await tblStream.ConcatenateStreamAsync(fileInfoStream.Stream);
        await tblStream.ConcatenateStreamAsync(filePathStream.Stream);

        return tblStream.ToByteArray();
    }
}
