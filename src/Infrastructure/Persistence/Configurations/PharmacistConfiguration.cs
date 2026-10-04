using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PharmacistConfiguration : IEntityTypeConfiguration<Pharmacist>
{
    public void Configure(EntityTypeBuilder<Pharmacist> builder)
    {
        builder.ToTable("Pharmacists");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200);
        builder.Property(x => x.LicenseNo).HasMaxLength(50);
        builder.Property(x => x.WorkTime).HasMaxLength(100);
        builder.Property(x => x.WorkType).HasMaxLength(100);
        builder.Property(x => x.PositionName).HasMaxLength(100);

        builder.HasIndex(x => x.LicenseNo);

        builder.HasOne(x => x.Pharmacy)
            .WithMany()
            .HasForeignKey(x => x.PharmacyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
