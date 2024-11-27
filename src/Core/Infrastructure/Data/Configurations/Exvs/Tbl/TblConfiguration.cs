using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TblEntity = BoostStudio.Domain.Entities.Exvs.Tbl.Tbl;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Tbl;

public class TblConfiguration : IEntityTypeConfiguration<TblEntity>
{
    public void Configure(EntityTypeBuilder<TblEntity> builder)
    {
        
    }
}
