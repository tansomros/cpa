using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class MTMReferConfiguration : IEntityTypeConfiguration<MTMRefer>
{
    public void Configure(EntityTypeBuilder<MTMRefer> builder)
    {
        builder.ToTable("MTMRefers");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.HospitalType).HasMaxLength(50);
        builder.Property(x => x.HospitalName).HasMaxLength(300);

        builder.HasOne<MTM>()
            .WithMany()
            .HasForeignKey(x => x.MTMUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
