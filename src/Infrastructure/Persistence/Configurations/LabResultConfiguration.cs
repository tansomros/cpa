using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class LabResultConfiguration : IEntityTypeConfiguration<LabResult>
{
    public void Configure(EntityTypeBuilder<LabResult> builder)
    {
        builder.ToTable("LabResults");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.ResultValue).HasMaxLength(200);
        builder.Property(x => x.IsNormal).HasMaxLength(10);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LabItem>()
            .WithMany()
            .HasForeignKey(x => x.LabUID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
