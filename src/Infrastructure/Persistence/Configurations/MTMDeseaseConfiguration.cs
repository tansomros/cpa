using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class MTMDeseaseConfiguration : IEntityTypeConfiguration<MTMDesease>
{
    public void Configure(EntityTypeBuilder<MTMDesease> builder)
    {
        builder.ToTable("MTMDeseases");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.ServiceTypeID).HasMaxLength(20);
        builder.Property(x => x.DeseaseOther).HasMaxLength(500);
        builder.Property(x => x.ICDCode).HasMaxLength(20);
        builder.Property(x => x.DeseaseName).HasMaxLength(300);

        builder.HasOne<MTM>()
            .WithMany()
            .HasForeignKey(x => x.MTMUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ServiceType>()
            .WithMany()
            .HasForeignKey(x => x.ServiceTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Desease>()
            .WithMany()
            .HasForeignKey(x => x.DeseaseUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
