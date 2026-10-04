using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class DispenseConfiguration : IEntityTypeConfiguration<Dispense>
{
    public void Configure(EntityTypeBuilder<Dispense> builder)
    {
        builder.ToTable("Dispenses");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.RefID).HasMaxLength(50);
        builder.Property(x => x.RefillDate).HasColumnType("date");
        builder.Property(x => x.LocationID).HasMaxLength(50);
        builder.Property(x => x.Remark).HasMaxLength(2000);
        builder.Property(x => x.TMTID).HasMaxLength(50);
        builder.Property(x => x.UOM).HasMaxLength(50);
        builder.Property(x => x.UsedRemark).HasMaxLength(500);
        builder.Property(x => x.CUser).HasMaxLength(100);
        builder.Property(x => x.MUser).HasMaxLength(100);

        builder.HasIndex(x => x.RefID);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MTM>()
            .WithMany()
            .HasForeignKey(x => x.MTMUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DrugMaster>()
            .WithMany()
            .HasForeignKey(x => x.DrugUID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
