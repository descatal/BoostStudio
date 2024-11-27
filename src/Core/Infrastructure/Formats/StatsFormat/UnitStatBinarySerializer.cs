using System.Buffers.Binary;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Exvs.Stats;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using BoostStudio.Infrastructure.Common.Constants;
using Kaitai;
using Crc32=System.IO.Hashing.Crc32;
using PropertiesEnum=BoostStudio.Formats.StatsBinaryFormat.PropertiesEnum;
using PropertyTypeEnum=BoostStudio.Formats.StatsBinaryFormat.PropertyTypeEnum;

namespace BoostStudio.Infrastructure.Formats.StatsFormat;

public class UnitStatBinarySerializer : IUnitStatBinarySerializer
{
    public async Task<byte[]> SerializeAsync(UnitStat data, CancellationToken cancellationToken)
    {
        await using var metadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        // Magic, it is not constant for most unit, for now use the hash of the unit's GameUnitId if the data is not present originally
        var magic = data.FileSignature ??
                    BinaryPrimitives.ReadUInt32BigEndian(
                        Crc32.Hash(
                            Encoding.Default.GetBytes(data.GameUnitId.ToString())
                            )
                        );

        metadataStream.WriteUint(magic);
        metadataStream.WriteUint(data.GameUnitId);
        metadataStream.WriteUint(0x1D7726BC); // Unknown constant

        await using var ammoIndexListStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var ammoHashListStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var statPropertiesHashStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var statPropertiesTypeStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var statSetOrderStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var statDataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        var ammoList = data.Ammo
            .OrderBy(ammo => ammo.Order)
            .ToList();
        var unitAmmoSlotList = data.AmmoSlots
            .OrderBy(ammoSlot => ammoSlot.SlotOrder)
            .ToList();

        // Ammo slots
        for (int i = 0; i < 5; i++)
        {
            var ammoHashIndex = -1;
            var unitAmmoSlot = unitAmmoSlotList.FirstOrDefault(slot => slot.SlotOrder == i);
            if (unitAmmoSlot is not null)
                ammoHashIndex = ammoList.FindIndex(ammo => ammo.Hash == unitAmmoSlot.AmmoHash);

            switch (i)
            {
                case < 4:
                    ammoIndexListStream.WriteInt(ammoHashIndex);
                    break;
                case 4:
                    {
                        // Originally 0, but repurposed as 5th ammo slot index
                        if (ammoHashIndex == 0)
                            throw new ArgumentException("5th ammo slot cannot have 0th index ammo!");

                        metadataStream.WriteInt(ammoHashIndex == -1 ? 0 : ammoHashIndex);
                        break;
                    }
            }
        }

        // Ammo hash list 
        ammoHashListStream.WriteUint((uint)ammoList.Count);
        foreach (var ammo in ammoList)
            ammoHashListStream.WriteUint(ammo.Hash);

        // Stat properties
        // To preserve the order, use the manually mapped stat properties
        var allStatProperties = StatProperties.OrderedStatProperties;
        statPropertiesHashStream.WriteUint((uint)allStatProperties.Count);
        foreach ((PropertiesEnum propertiesEnum, PropertyTypeEnum type) in allStatProperties)
        {
            statPropertiesHashStream.WriteUint((uint)propertiesEnum);

            // Except for Unk608, which is a special case
            if (propertiesEnum != PropertiesEnum.Unk608)
                statPropertiesTypeStream.WriteUint((uint)type);
            else
                statPropertiesTypeStream.WriteUint((uint)PropertyTypeEnum.Unknown6);
        }

        // Stat set order 
        var statSets = data.Stats
            .OrderBy(stat => stat.Order)
            .ToList();

        statSetOrderStream.WriteUint((uint)statSets.Count);
        foreach (var stat in statSets)
            statSetOrderStream.WriteUint((uint)stat.Order);

        // Stat data
        foreach (var stat in statSets)
        {
            statDataStream.WriteInt(stat.UnitCost);
            statDataStream.WriteInt(stat.UnitCost2);
            statDataStream.WriteInt(stat.MaxHp);
            statDataStream.WriteInt(stat.DownValueThreshold);
            statDataStream.WriteInt(stat.YorukeValueThreshold);
            statDataStream.WriteInt(stat.Unk20);
            statDataStream.WriteInt(stat.Unk24);
            statDataStream.WriteInt(stat.Unk28);
            statDataStream.WriteInt(stat.MaxBoost);
            statDataStream.WriteFloat(stat.Unk36);
            statDataStream.WriteFloat(stat.Unk40);
            statDataStream.WriteInt(stat.Unk44);
            statDataStream.WriteFloat(stat.GravityMultiplierAir);
            statDataStream.WriteFloat(stat.GravityMultiplierLand);
            statDataStream.WriteFloat(stat.Unk56);
            statDataStream.WriteFloat(stat.Unk60);
            statDataStream.WriteFloat(stat.Unk64);
            statDataStream.WriteFloat(stat.Unk68);
            statDataStream.WriteFloat(stat.Unk72);
            statDataStream.WriteFloat(stat.Unk76);
            statDataStream.WriteFloat(stat.Unk80);
            statDataStream.WriteFloat(stat.CameraZoomMultiplier);
            statDataStream.WriteFloat(stat.Unk88);
            statDataStream.WriteFloat(stat.Unk92);
            statDataStream.WriteFloat(stat.Unk96);
            statDataStream.WriteFloat(stat.Unk100);
            statDataStream.WriteFloat(stat.Unk104);
            statDataStream.WriteFloat(stat.Unk108);
            statDataStream.WriteFloat(stat.SizeMultiplier);
            statDataStream.WriteFloat(stat.Unk116);
            statDataStream.WriteFloat(stat.Unk120);
            statDataStream.WriteFloat(stat.Unk124);
            statDataStream.WriteFloat(stat.Unk128);
            statDataStream.WriteFloat(stat.Unk132);
            statDataStream.WriteFloat(stat.Unk136);
            statDataStream.WriteFloat(stat.Unk140);
            statDataStream.WriteFloat(stat.Unk144);
            statDataStream.WriteFloat(stat.Unk148);
            statDataStream.WriteFloat(stat.Unk152);
            statDataStream.WriteFloat(stat.Unk156);
            statDataStream.WriteFloat(stat.Unk160);
            statDataStream.WriteFloat(stat.Unk164);
            statDataStream.WriteFloat(stat.Unk168);
            statDataStream.WriteFloat(stat.Unk172);
            statDataStream.WriteFloat(stat.Unk176);
            statDataStream.WriteFloat(stat.Unk180);
            statDataStream.WriteFloat(stat.Unk184);
            statDataStream.WriteFloat(stat.RedLockRangeMelee);
            statDataStream.WriteFloat(stat.RedLockRange);
            statDataStream.WriteFloat(stat.Unk196);
            statDataStream.WriteFloat(stat.Unk200);
            statDataStream.WriteInt(stat.Unk204);
            statDataStream.WriteInt(stat.Unk208);
            statDataStream.WriteInt(stat.BoostReplenish);
            statDataStream.WriteInt(stat.Unk216);
            statDataStream.WriteInt(stat.BoostInitialConsumption);
            statDataStream.WriteInt(stat.BoostFuwaInitialConsumption);
            statDataStream.WriteInt(stat.BoostFlyConsumption);
            statDataStream.WriteInt(stat.BoostGroundStepInitialConsumption);
            statDataStream.WriteInt(stat.BoostGroundStepConsumption);
            statDataStream.WriteInt(stat.BoostAirStepInitialConsumption);
            statDataStream.WriteInt(stat.BoostAirStepConsumption);
            statDataStream.WriteInt(stat.BoostBdInitialConsumption);
            statDataStream.WriteInt(stat.BoostBdConsumption);
            statDataStream.WriteInt(stat.Unk256);
            statDataStream.WriteInt(stat.Unk260);
            statDataStream.WriteInt(stat.Unk264);
            statDataStream.WriteInt(stat.Unk268);
            statDataStream.WriteInt(stat.BoostTransformInitialConsumption);
            statDataStream.WriteInt(stat.BoostTransformConsumption);
            statDataStream.WriteInt(stat.BoostNonVernierActionConsumption);
            statDataStream.WriteInt(stat.BoostPostActionConsumption);
            statDataStream.WriteInt(stat.BoostRainbowStepInitialConsumption);
            statDataStream.WriteInt(stat.Unk292);
            statDataStream.WriteInt(stat.Unk296);
            statDataStream.WriteInt(stat.Unk300);
            statDataStream.WriteInt(stat.Unk304);
            statDataStream.WriteInt(stat.Unk308);
            statDataStream.WriteInt(stat.Unk312);
            statDataStream.WriteInt(stat.Unk316);
            statDataStream.WriteInt(stat.Unk320);
            statDataStream.WriteInt(stat.Unk324);
            statDataStream.WriteInt(stat.Unk328);
            statDataStream.WriteInt(stat.Unk332);
            statDataStream.WriteFloat(stat.AssaultBurstRedLockMelee);
            statDataStream.WriteFloat(stat.AssaultBurstRedLock);
            statDataStream.WriteFloat(stat.AssaultBurstDamageDealtMultiplier);
            statDataStream.WriteFloat(stat.AssaultBurstDamageTakenMultiplier);
            statDataStream.WriteFloat(stat.AssaultBurstMobilityMultiplier);
            statDataStream.WriteFloat(stat.AssaultBurstDownValueDealtMultiplier);
            statDataStream.WriteFloat(stat.AssaultBurstBoostConsumptionMultiplier);
            statDataStream.WriteInt(stat.Unk364);
            statDataStream.WriteInt(stat.Unk368);
            statDataStream.WriteFloat(stat.AssaultBurstDamageDealtBurstGaugeIncreaseMultiplier);
            statDataStream.WriteFloat(stat.AssaultBurstDamageTakenBurstGaugeIncreaseMultiplier);
            statDataStream.WriteInt(stat.Unk380);
            statDataStream.WriteFloat(stat.Unk384);
            statDataStream.WriteFloat(stat.Unk388);
            statDataStream.WriteFloat(stat.Unk392);
            statDataStream.WriteFloat(stat.Unk396);
            statDataStream.WriteFloat(stat.BlastBurstRedLockMelee);
            statDataStream.WriteFloat(stat.BlastBurstRedLock);
            statDataStream.WriteFloat(stat.BlastBurstDamageDealtMultiplier);
            statDataStream.WriteFloat(stat.BlastBurstDamageTakenMultiplier);
            statDataStream.WriteFloat(stat.BlastBurstMobilityMultiplier);
            statDataStream.WriteFloat(stat.BlastBurstDownValueDealtMultiplier);
            statDataStream.WriteFloat(stat.BlastBurstBoostConsumptionMultiplier);
            statDataStream.WriteInt(stat.Unk428);
            statDataStream.WriteInt(stat.Unk432);
            statDataStream.WriteFloat(stat.BlastBurstDamageDealtBurstGaugeIncreaseMultiplier);
            statDataStream.WriteFloat(stat.BlastBurstDamageTakenBurstGaugeIncreaseMultiplier);
            statDataStream.WriteInt(stat.Unk444);
            statDataStream.WriteFloat(stat.Unk448);
            statDataStream.WriteFloat(stat.Unk452);
            statDataStream.WriteFloat(stat.Unk456);
            statDataStream.WriteFloat(stat.Unk460);
            statDataStream.WriteFloat(stat.ThirdBurstRedLockMelee);
            statDataStream.WriteFloat(stat.ThirdBurstRedLock);
            statDataStream.WriteFloat(stat.ThirdBurstDamageDealtMultiplier);
            statDataStream.WriteFloat(stat.ThirdBurstDamageTakenMultiplier);
            statDataStream.WriteFloat(stat.ThirdBurstMobilityMultiplier);
            statDataStream.WriteFloat(stat.ThirdBurstDownValueDealtMultiplier);
            statDataStream.WriteFloat(stat.ThirdBurstBoostConsumptionMultiplier);
            statDataStream.WriteInt(stat.Unk492);
            statDataStream.WriteInt(stat.Unk496);
            statDataStream.WriteFloat(stat.ThirdBurstDamageDealtBurstGaugeIncreaseMultiplier);
            statDataStream.WriteFloat(stat.ThirdBurstDamageTakenBurstGaugeIncreaseMultiplier);
            statDataStream.WriteInt(stat.Unk508);
            statDataStream.WriteFloat(stat.Unk512);
            statDataStream.WriteFloat(stat.Unk516);
            statDataStream.WriteFloat(stat.Unk520);
            statDataStream.WriteFloat(stat.Unk524);
            statDataStream.WriteFloat(stat.FourthBurstRedLockMelee);
            statDataStream.WriteFloat(stat.FourthBurstRedLock);
            statDataStream.WriteFloat(stat.FourthBurstDamageDealtMultiplier);
            statDataStream.WriteFloat(stat.FourthBurstDamageTakenMultiplier);
            statDataStream.WriteFloat(stat.FourthBurstMobilityMultiplier);
            statDataStream.WriteFloat(stat.FourthBurstDownValueDealtMultiplier);
            statDataStream.WriteFloat(stat.FourthBurstBoostConsumptionMultiplier);
            statDataStream.WriteInt(stat.Unk572);
            statDataStream.WriteInt(stat.Unk576);
            statDataStream.WriteFloat(stat.FourthBurstDamageDealtBurstGaugeIncreaseMultiplier);
            statDataStream.WriteFloat(stat.FourthBurstDamageTakenBurstGaugeIncreaseMultiplier);
            statDataStream.WriteInt(stat.Unk588);
            statDataStream.WriteFloat(stat.Unk592);
            statDataStream.WriteFloat(stat.Unk596);
            statDataStream.WriteFloat(stat.Unk600);
            statDataStream.WriteFloat(stat.Unk604);
            statDataStream.WriteInt(stat.Unk608);
        }

        // Rewrite metadata pointer header
        var pointer = (uint)(metadataStream.GetLength() + ammoIndexListStream.GetLength() + 0x10); // 0x10 is this pointer section size
        metadataStream.WriteUint(pointer);

        pointer += (uint)(ammoHashListStream.GetLength());
        metadataStream.WriteUint(pointer);

        pointer += (uint)(statPropertiesHashStream.GetLength() + statPropertiesTypeStream.GetLength());
        metadataStream.WriteUint(pointer);

        pointer += (uint)(statSetOrderStream.GetLength());
        metadataStream.WriteUint(pointer);

        // Concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        await fileStream.ConcatenateStreamAsync(ammoIndexListStream.Stream);
        await fileStream.ConcatenateStreamAsync(ammoHashListStream.Stream);
        await fileStream.ConcatenateStreamAsync(statPropertiesHashStream.Stream);
        await fileStream.ConcatenateStreamAsync(statPropertiesTypeStream.Stream);
        await fileStream.ConcatenateStreamAsync(statSetOrderStream.Stream);
        await fileStream.ConcatenateStreamAsync(statDataStream.Stream);

        return fileStream.ToByteArray();
    }

    public async Task<StatsBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new StatsBinaryFormat(kaitaiStream);
        return deserializedObject;
    }
}
