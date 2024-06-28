using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using BoostStudio.Infrastructure.Common.Constants;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.ProjectileFormat;

public class UnitProjectileBinarySerializer : IUnitProjectileBinarySerializer
{
    public async Task<byte[]> SerializeAsync(UnitProjectile data, CancellationToken cancellationToken)
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
        metadataStream.WriteUint(0x67F019CE); // Unknown constant, differ between fb and mbon
        metadataStream.WriteUint(data.GameUnitId);
        
        await using var projectileHashStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var projectileDataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        
        projectileHashStream.WriteUint((uint)data.Projectiles.Count);
        
        foreach (var projectile in data.Projectiles)
        {
            projectileHashStream.WriteUint(projectile.Hash);
            
            projectileDataStream.WriteUint(projectile.ProjectileType);
            projectileDataStream.WriteUint(projectile.HitboxHash ?? 0);
            projectileDataStream.WriteUint(projectile.ModelHash);
            projectileDataStream.WriteUint(projectile.SkeletonIndex);
            projectileDataStream.WriteUint(projectile.AimType);
            projectileDataStream.WriteFloat(projectile.TranslateY);
            projectileDataStream.WriteFloat(projectile.TranslateZ);
            projectileDataStream.WriteFloat(projectile.TranslateX);
            projectileDataStream.WriteFloat(projectile.RotateX);
            projectileDataStream.WriteFloat(projectile.RotateZ);
            projectileDataStream.WriteUint(projectile.CosmeticHash);
            projectileDataStream.WriteUint(projectile.Unk44);
            projectileDataStream.WriteUint(projectile.Unk48);
            projectileDataStream.WriteUint(projectile.Unk52);
            projectileDataStream.WriteUint(projectile.Unk56);
            projectileDataStream.WriteUint(projectile.AmmoConsumption);
            projectileDataStream.WriteUint(projectile.DurationFrame);
            projectileDataStream.WriteFloat(projectile.MaxTravelDistance);
            projectileDataStream.WriteFloat(projectile.InitialSpeed);
            projectileDataStream.WriteFloat(projectile.Acceleration);
            projectileDataStream.WriteUint(projectile.AccelerationStartFrame);
            projectileDataStream.WriteFloat(projectile.Unk84);
            projectileDataStream.WriteFloat(projectile.MaxSpeed);
            projectileDataStream.WriteFloat(projectile.Reserved92);
            projectileDataStream.WriteFloat(projectile.Reserved96);
            projectileDataStream.WriteFloat(projectile.Reserved100);
            projectileDataStream.WriteFloat(projectile.Reserved104);
            projectileDataStream.WriteFloat(projectile.Reserved108);
            projectileDataStream.WriteFloat(projectile.Reserved112);
            projectileDataStream.WriteUint(projectile.Reserved116);
            projectileDataStream.WriteFloat(projectile.HorizontalGuidance);
            projectileDataStream.WriteFloat(projectile.HorizontalGuidanceAngle);
            projectileDataStream.WriteFloat(projectile.VerticalGuidance);
            projectileDataStream.WriteFloat(projectile.VerticalGuidanceAngle);
            projectileDataStream.WriteUint(projectile.Reserved136);
            projectileDataStream.WriteUint(projectile.Reserved140);
            projectileDataStream.WriteFloat(projectile.Reserved144);
            projectileDataStream.WriteFloat(projectile.Reserved148);
            projectileDataStream.WriteFloat(projectile.Reserved152);
            projectileDataStream.WriteFloat(projectile.Reserved156);
            projectileDataStream.WriteFloat(projectile.Reserved160);
            projectileDataStream.WriteFloat(projectile.Reserved164);
            projectileDataStream.WriteUint(projectile.Reserved168);
            projectileDataStream.WriteFloat(projectile.Reserved172);
            projectileDataStream.WriteFloat(projectile.Size);
            projectileDataStream.WriteUint(projectile.Reserved180);
            projectileDataStream.WriteUint(projectile.Reserved184);
            projectileDataStream.WriteUint(projectile.SoundEffectHash);
            projectileDataStream.WriteUint(projectile.Reserved192);
            projectileDataStream.WriteUint(projectile.Reserved196);
            projectileDataStream.WriteUint(projectile.ChainedProjectileHash);
            projectileDataStream.WriteUint(projectile.Reserved204);
            projectileDataStream.WriteUint(projectile.Reserved208);
            projectileDataStream.WriteUint(projectile.Reserved212);
            projectileDataStream.WriteUint(projectile.Reserved216);
            projectileDataStream.WriteFloat(projectile.Reserved220);
            projectileDataStream.WriteFloat(projectile.Reserved224);
            projectileDataStream.WriteFloat(projectile.Reserved228);
            projectileDataStream.WriteFloat(projectile.Reserved232);
            projectileDataStream.WriteFloat(projectile.Reserved236);
            projectileDataStream.WriteFloat(projectile.Reserved240);
            projectileDataStream.WriteFloat(projectile.Reserved244);
            projectileDataStream.WriteFloat(projectile.Reserved248);
            projectileDataStream.WriteFloat(projectile.Reserved252);
            projectileDataStream.WriteFloat(projectile.Reserved256);
            projectileDataStream.WriteFloat(projectile.Reserved260);
            projectileDataStream.WriteFloat(projectile.Reserved264);
            projectileDataStream.WriteFloat(projectile.Reserved268);
            projectileDataStream.WriteFloat(projectile.Reserved272);
            projectileDataStream.WriteFloat(projectile.Reserved276);
        }
        
        // rewrite write pointer to data in metadata header
        var dataPointer = (uint)(metadataStream.GetLength() + projectileHashStream.GetLength() + 0x4); // 0x4 as the size for this pointer
        metadataStream.WriteUint(dataPointer);
        
        // concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        await fileStream.ConcatenateStreamAsync(projectileHashStream.Stream);
        await fileStream.ConcatenateStreamAsync(projectileDataStream.Stream);
        
        return fileStream.ToByteArray();
    }

    public async Task<ProjectileBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new ProjectileBinaryFormat(kaitaiStream);
        return deserializedObject;
    }
}
