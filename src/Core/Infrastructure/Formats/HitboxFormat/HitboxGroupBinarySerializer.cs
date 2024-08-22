using System.Buffers.Binary;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Common;
using BoostStudio.Infrastructure.Common.Constants;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.HitboxFormat;

public class HitboxGroupBinarySerializer : IHitboxGroupBinarySerializer
{
    public async Task<byte[]> SerializeAsync(HitboxGroup data, CancellationToken cancellationToken)
    {
        await using var metadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        
        metadataStream.WriteUint(data.Hash);
        
        await using var hitboxPropertiesHashStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var hitboxDataHashStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await using var hitboxDataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        var allProperties = Enum
            .GetValues<HitboxBinaryFormat.PropertiesEnum>()
            .OrderBy(propertiesEnum => HitboxProperties.HitboxPropertiesEnumOrder.IndexOf(propertiesEnum))
            .ToList();
        
        hitboxPropertiesHashStream.WriteUint((uint)allProperties.Count);
        foreach (var propertiesEnum in allProperties)
        {
            hitboxPropertiesHashStream.WriteUint((uint)propertiesEnum);
        }
        
        hitboxDataHashStream.WriteUint((uint)data.Hitboxes.Count);
        foreach (var hitbox in data.Hitboxes)
        {
            hitboxDataHashStream.WriteUint(hitbox.Hash);

            var allHitboxProperties = hitbox.GetType().GetProperties();
            foreach (var property in allProperties)
            {
                var matchingProperty = allHitboxProperties
                    .FirstOrDefault(propertyInfo => propertyInfo.Name.Equals(Enum.GetName(property), StringComparison.OrdinalIgnoreCase));

                if (matchingProperty is null)
                    continue;

                var matchingPropertyType = matchingProperty.PropertyType;
                switch (Type.GetTypeCode(matchingPropertyType))
                {
                    case TypeCode.UInt32:
                        hitboxDataStream.WriteUint((uint)matchingProperty.GetValue(hitbox)!);
                        break;
                    case TypeCode.Single:
                        hitboxDataStream.WriteFloat((float)matchingProperty.GetValue(hitbox)!);
                        break;
                }
            }
        }
        
        // rewrite write pointer to data in metadata header
        var dataHashPointer = 0x10 + hitboxPropertiesHashStream.GetLength();
        var dataPointer = dataHashPointer + hitboxDataHashStream.GetLength();
        
        metadataStream.WriteUint((uint)dataHashPointer);
        metadataStream.WriteUint((uint)dataPointer);
        metadataStream.WriteUint(0u);
        
        // concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        await fileStream.ConcatenateStreamAsync(hitboxPropertiesHashStream.Stream);
        await fileStream.ConcatenateStreamAsync(hitboxDataHashStream.Stream);
        await fileStream.ConcatenateStreamAsync(hitboxDataStream.Stream);
        
        return fileStream.ToByteArray();
    }
    
    public Task<HitboxBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new HitboxBinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }
}
