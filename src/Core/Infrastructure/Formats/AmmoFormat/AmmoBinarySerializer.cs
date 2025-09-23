using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Exvs.Ammo;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using Kaitai;
using AmmoMapper = BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Infrastructure.Formats.AmmoFormat;

public class AmmoBinarySerializer : IFormatBinarySerializer<List<Ammo>>
{
    public async Task<byte[]> SerializeAsync(List<Ammo> data, CancellationToken cancellationToken)
    {
        await using var metadataStream = new CustomBinaryWriter(
            new MemoryStream(),
            Endianness.BigEndian
        );

        metadataStream.WriteUint(0x1D258FF7); // Magic
        metadataStream.WriteUint(0x22); // Fixed property count, is the actual property count - 1
        metadataStream.WriteUint(0); // Two zero 4 byte array
        metadataStream.WriteUint(0);
        metadataStream.WriteUint((uint)data.Count);

        await using var ammoHashListStream = new CustomBinaryWriter(
            new MemoryStream(),
            Endianness.BigEndian
        );
        await using var ammoPropertiesStream = new CustomBinaryWriter(
            new MemoryStream(),
            Endianness.BigEndian
        );

        foreach (var ammo in data)
        {
            ammoHashListStream.WriteUint(ammo.Hash);
            ammoPropertiesStream.WriteUint(ammo.AmmoType);
            ammoPropertiesStream.WriteUint(ammo.MaxAmmo);
            ammoPropertiesStream.WriteUint(ammo.InitialAmmo);
            ammoPropertiesStream.WriteUint(ammo.TimedDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.Unk16);
            ammoPropertiesStream.WriteUint(ammo.ReloadType);
            ammoPropertiesStream.WriteUint(ammo.CooldownDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.ReloadDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.AssaultBurstReloadDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.BlastBurstReloadDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.Unk40);
            ammoPropertiesStream.WriteUint(ammo.Unk44);
            ammoPropertiesStream.WriteUint(ammo.InactiveUnk48);
            ammoPropertiesStream.WriteUint(ammo.InactiveCooldownDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.InactiveReloadDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.InactiveAssaultBurstReloadDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.InactiveBlastBurstReloadDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.InactiveUnk68);
            ammoPropertiesStream.WriteUint(ammo.InactiveUnk72);
            ammoPropertiesStream.WriteUint(ammo.BurstReplenish);
            ammoPropertiesStream.WriteUint(ammo.Unk80);
            ammoPropertiesStream.WriteUint(ammo.Unk84);
            ammoPropertiesStream.WriteUint(ammo.Unk88);
            ammoPropertiesStream.WriteUint(ammo.ChargeInput);
            ammoPropertiesStream.WriteUint(ammo.ChargeDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.AssaultBurstChargeDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.BlastBurstChargeDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.Unk108);
            ammoPropertiesStream.WriteUint(ammo.Unk112);
            ammoPropertiesStream.WriteUint(ammo.ReleaseChargeLingerDurationFrame);
            ammoPropertiesStream.WriteUint(ammo.MaxChargeLevel);
            ammoPropertiesStream.WriteUint(ammo.Unk124);
            ammoPropertiesStream.WriteUint(ammo.ChargeMultiLockFlag);
        }

        // Concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(
            new MemoryStream(),
            Endianness.BigEndian
        );
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        await fileStream.ConcatenateStreamAsync(ammoHashListStream.Stream);
        await fileStream.ConcatenateStreamAsync(ammoPropertiesStream.Stream);

        return fileStream.ToByteArray();
    }

    public Task<List<Ammo>> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new AmmoBinaryFormat(kaitaiStream);

        // Map deserializedAmmoBinaryObject into Ammo
        var ammo = AmmoMapper.AmmoBinaryFormatToAmmo(deserializedObject).ToList();

        return Task.FromResult(ammo);
    }
}
