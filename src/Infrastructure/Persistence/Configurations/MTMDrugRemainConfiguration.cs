using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class MTMDrugRemainConfiguration : IEntityTypeConfiguration<MTMDrugRemain>
{
    public void Configure(EntityTypeBuilder<MTMDrugRemain> builder)
    {
        builder.ToTable("MTMDrugRemains");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.ServiceTypeID).HasMaxLength(20);
        builder.Property(x => x.UOM).HasMaxLength(50);
        builder.Property(x => x.CUser).HasMaxLength(100);
        builder.Property(x => x.MUser).HasMaxLength(100);
        builder.Property(x => x.RemainFrom).HasMaxLength(100);
        builder.Property(x => x.ReasonRemark).HasMaxLength(500);
        builder.Property(x => x.TMTID).HasMaxLength(50);

        builder.HasOne<MTM>()
            .WithMany()
            .HasForeignKey(x => x.MTMUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ServiceType>()
            .WithMany()
            .HasForeignKey(x => x.ServiceTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DrugMaster>()
            .WithMany()
            .HasForeignKey(x => x.DrugUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
