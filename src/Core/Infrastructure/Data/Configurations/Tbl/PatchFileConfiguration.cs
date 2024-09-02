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

        builder.HasOne<TblEntity>()
            .WithMany(tbl => tbl.PatchFiles)
            .HasForeignKey(patchFile => patchFile.TblId)
            .IsRequired(false);

        builder.OwnsOne<PatchFileInfo>(patchFile => patchFile.FileInfo, patchFileInfoBuilder =>
        {
            patchFileInfoBuilder.HasOne<AssetFile>(patchFileInfo => patchFileInfo.AssetFile)
                .WithMany()
                .HasForeignKey(patchFileInfo => patchFileInfo.AssetFileHash)
                .IsRequired();
        });
    }
}

// public class PatchFileInfoConfiguration : IEntityTypeConfiguration<PatchFileInfo>
// {
//     public void Configure(EntityTypeBuilder<PatchFileInfo> builder)
//     {
//         builder.HasOne<AssetFile>()
//             .WithMany(file => file.PatchFiles)
//             .HasForeignKey(file => file.AssetFileHash)
//             .IsRequired(false);
//     }
// }
