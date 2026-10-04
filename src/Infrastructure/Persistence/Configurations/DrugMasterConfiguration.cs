using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class DrugMasterConfiguration : IEntityTypeConfiguration<DrugMaster>
{
    public void Configure(EntityTypeBuilder<DrugMaster> builder)
    {
        builder.ToTable("DrugMasters");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.TMTID).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(500);
        builder.Property(x => x.AliasName).HasMaxLength(500);
        builder.Property(x => x.Manufacturer).HasMaxLength(300);
        builder.Property(x => x.FSN).HasMaxLength(1000);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);

        builder.HasIndex(x => x.TMTID);
        builder.HasIndex(x => x.Name);
    }
}
