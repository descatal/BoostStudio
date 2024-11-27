using BoostStudio.Domain.Entities.Exvs.Assets;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Assets;

[Mapper]
public static partial class AssetFileMapper
{
    public static partial IQueryable<AssetFileVm> ProjectToVm(IQueryable<AssetFile> entity);
    
    public static partial AssetFileVm ToVm(AssetFile entity);
    
    public static partial AssetFile ToEntity(AssetFileDto dto);
    
    public static partial void Update(AssetFileDto source, AssetFile target);
}
