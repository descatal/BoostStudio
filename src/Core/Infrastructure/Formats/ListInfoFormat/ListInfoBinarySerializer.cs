using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Domain.Entities.Exvs.Units;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.ListInfoFormat;

public class ListInfoBinarySerializer : IListInfoBinarySerializer
{
    public async Task<byte[]> SerializePlayableSeriesAsync(
        List<Series> data,
        CancellationToken cancellationToken = default)
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

    public async Task<byte[]> SerializePlayableCharactersAsync(
        List<Unit> data,
        CancellationToken cancellationToken = default)
    {
        await using var metadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var dataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var stringSectionStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        Dictionary<string, uint> stringPointerMap = [];

        var playableCharactersCount = data.Count(unit => unit.PlayableCharacter is not null);
        var stringSectionPointer = (uint)(0x8 + playableCharactersCount * 0xA0);

        WriteStringSection(stringSectionPointer, stringPointerMap, "SCharacterList", stringSectionStream, metadataStream);
        metadataStream.WriteUshort((ushort)playableCharactersCount);
        metadataStream.WriteUshort(0);

        foreach (var unit in data)
        {
            if (unit.PlayableCharacter is null)
                continue;

            dataStream.WriteByte(unit.PlayableCharacter.UnitIndex);
            dataStream.WriteByte(unit.PlayableCharacter.SeriesId);
            dataStream.WriteUshort(unit.PlayableCharacter.Unk2);
            dataStream.WriteUint(unit.GameUnitId);

            // pointer to release string
            WriteStringSection(stringSectionPointer, stringPointerMap, "リリース", stringSectionStream, dataStream, encoding: Encoding.UTF8);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.FString, stringSectionStream, dataStream);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.FOutString, stringSectionStream, dataStream);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.PString, stringSectionStream, dataStream);

            dataStream.WriteByte(unit.PlayableCharacter.UnitSelectOrderInSeries);
            dataStream.WriteByte(unit.PlayableCharacter.ArcadeSmallSpriteIndex);
            dataStream.WriteByte(unit.PlayableCharacter.ArcadeUnitNameSpriteIndex);
            dataStream.WriteByte(unit.PlayableCharacter.Unk27);

            WriteAssetHash(unit, dataStream, AssetFileType.ArcadeSelectionCostume1Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.ArcadeSelectionCostume2Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.ArcadeSelectionCostume3Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingLeftCostume1Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingLeftCostume2Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingLeftCostume3Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingRightCostume1Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingRightCostume2Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingRightCostume3Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.GenericSelectionCostume1Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.GenericSelectionCostume2Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.GenericSelectionCostume3Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingTargetUnitSprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingTargetPilotCostume1Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingTargetPilotCostume2Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingTargetPilotCostume3Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.InGameSortieAndAwakeningPilotCostume1Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.InGameSortieAndAwakeningPilotCostume2Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.InGameSortieAndAwakeningPilotCostume3Sprite);
            WriteAssetHash(unit, dataStream, AssetFileType.SpriteFrames);
            WriteAssetHash(unit, dataStream, AssetFileType.ResultSmallUnitSprite);

            dataStream.WriteByte(unit.PlayableCharacter.Unk112);
            dataStream.WriteByte(unit.PlayableCharacter.FigurineSpriteIndex);
            dataStream.WriteUshort(unit.PlayableCharacter.Unk114);

            WriteAssetHash(unit, dataStream, AssetFileType.FigurineSprite);
            WriteAssetHash(unit, dataStream, AssetFileType.LoadingTargetUnitSmallSprite);

            dataStream.WriteUint(unit.PlayableCharacter.Unk124);
            dataStream.WriteUint(unit.PlayableCharacter.Unk128);

            WriteAssetHash(unit, dataStream, AssetFileType.CatalogStorePilotCostume2Sprite);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.CatalogStorePilotCostume2TString, stringSectionStream, dataStream);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.CatalogStorePilotCostume2String, stringSectionStream, dataStream);

            WriteAssetHash(unit, dataStream, AssetFileType.CatalogStorePilotCostume3Sprite);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.CatalogStorePilotCostume3TString, stringSectionStream, dataStream);
            WriteStringSection(stringSectionPointer, stringPointerMap, unit.PlayableCharacter.CatalogStorePilotCostume3String, stringSectionStream, dataStream);

            dataStream.WriteUint(unit.PlayableCharacter.Unk156);
        }

        // concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        await fileStream.ConcatenateStreamAsync(dataStream.Stream);
        await fileStream.ConcatenateStreamAsync(stringSectionStream.Stream);

        return fileStream.ToByteArray();
    }

    public Task<ListInfoBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken = default)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new ListInfoBinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }

    private static void WriteAssetHash(
        Unit unit,
        CustomBinaryWriter dataStream,
        AssetFileType type)
    {
        var assetHash = unit.AssetFiles.FirstOrDefault(file => file.FileType.Contains(type))?.Hash ?? 0;
        dataStream.WriteUint(assetHash);
    }

    private static void WriteStringSection(
        uint stringSectionPointer,
        Dictionary<string, uint> stringPointerMap,
        string? str,
        CustomBinaryWriter stringSectionStream,
        CustomBinaryWriter dataStream,
        Encoding? encoding = null)
    {
        uint stringPointer = stringSectionPointer;

        if (!string.IsNullOrWhiteSpace(str))
        {
            stringPointer += (uint)stringSectionStream.GetLength();

            // only writes if there's a new string
            if (stringPointerMap.TryGetValue(str, out var existingStringPointer))
                stringPointer = existingStringPointer;
            else
                stringSectionStream.WriteString(str, encoding ?? Encoding.Default, writeSize: false);

            stringPointerMap[str] = stringPointer;
        }

        dataStream.WriteUint(stringPointer);
    }
}
