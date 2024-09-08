using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Domain.Entities.Unit.Assets;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TblEntity=BoostStudio.Domain.Entities.Tbl.Tbl;

namespace BoostStudio.Infrastructure.Data.Configurations.Tbl;

public class PatchFileConfiguration : IEntityTypeConfiguration<PatchFile>
{
    public void Configure(EntityTypeBuilder<PatchFile> builder)
    {
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();

        builder.HasOne<TblEntity>(patchFile => patchFile.Tbl)
            .WithMany(tbl => tbl.PatchFiles)
            .HasForeignKey(patchFile => patchFile.TblId)
            .IsRequired();

        // this should not be here, but inside OwnsOne builder configuration
        // ef does not support owned relationship yet, so this is the only way to do it
        // will need to manually make sure this AssetFile is null when PatchFileInfo is null too
        builder.HasOne<AssetFile>(patchFileInfo => patchFileInfo.AssetFile)
            .WithMany(assetFile => assetFile.PatchFiles)
            .HasForeignKey(patchFileInfo => patchFileInfo.AssetFileHash)
            .HasPrincipalKey(assetFile => assetFile.Hash)
            .IsRequired(false);
        
        builder.OwnsOne<PathInfo>(patchFile => patchFile.PathInfo);
        builder.OwnsOne<PatchFileInfo>(patchFile => patchFile.FileInfo);
    }
}
