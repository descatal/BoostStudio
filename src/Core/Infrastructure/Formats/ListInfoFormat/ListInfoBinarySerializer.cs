using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.ListInfoFormat;

public class ListInfoBinarySerializer : IListInfoBinarySerializer
{
    public async Task<byte[]> SerializePlayableSeriesAsync(
        List<Series> data,
        CancellationToken cancellationToken)
    {
        await using var metadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var dataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var stringSectionStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        stringSectionStream.WriteString("SSeriesList", Encoding.Default, writeSize: false);
        var seriesListNameStringLength = stringSectionStream.GetLength();
        stringSectionStream.WriteString("リリース", Encoding.UTF8, writeSize: false);

        var stringSectionPointer = (uint)(0x8 + data.Count * 0x10);
        var configurationPointer = (uint)(stringSectionPointer + seriesListNameStringLength);

        metadataStream.WriteUint(stringSectionPointer);
        metadataStream.WriteUshort((ushort)data.Count);
        metadataStream.WriteUshort(0);

        foreach (var series in data)
        {
            if (series.PlayableSeries is null)
                continue;

            dataStream.WriteByte(series.Id);
            dataStream.WriteByte(series.PlayableSeries.Unk2);
            dataStream.WriteByte(series.PlayableSeries.Unk3);
            dataStream.WriteByte(series.PlayableSeries.Unk4);

            // pointer to release string
            dataStream.WriteUint(configurationPointer);

            dataStream.WriteByte(series.PlayableSeries.SelectOrder);
            dataStream.WriteByte(series.PlayableSeries.LogoSpriteIndex);
            dataStream.WriteByte(series.PlayableSeries.LogoSprite2Index);
            dataStream.WriteByte(series.PlayableSeries.Unk11);

            dataStream.WriteUint(series.PlayableSeries.MovieAssetHash ?? 0);
        }

        // concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        await fileStream.ConcatenateStreamAsync(dataStream.Stream);
        await fileStream.ConcatenateStreamAsync(stringSectionStream.Stream);

        return fileStream.ToByteArray();
    }

    public Task<ListInfoBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new ListInfoBinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }
}
