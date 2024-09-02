using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TblEntity = BoostStudio.Domain.Entities.Tbl.Tbl;

namespace BoostStudio.Infrastructure.Data.Configurations.Tbl;

public class TblConfiguration : IEntityTypeConfiguration<TblEntity>
{
    public void Configure(EntityTypeBuilder<TblEntity> builder)
    {
        
    }
}
