using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class MTMConfiguration : IEntityTypeConfiguration<MTM>
{
    public void Configure(EntityTypeBuilder<MTM> builder)
    {
        builder.ToTable("MTMs");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.LocationID).HasMaxLength(50);
        builder.Property(x => x.MTMTYPE).HasMaxLength(20);
        builder.Property(x => x.PFROM).HasMaxLength(50);
        builder.Property(x => x.FROMTXT).HasMaxLength(500);
        builder.Property(x => x.HospitalName).HasMaxLength(300);
        builder.Property(x => x.CreateBy).HasMaxLength(100);
        builder.Property(x => x.UpdBy).HasMaxLength(100);
        builder.Property(x => x.isPitting).HasMaxLength(10);
        builder.Property(x => x.isWound).HasMaxLength(10);
        builder.Property(x => x.isPeripheral).HasMaxLength(10);
        builder.Property(x => x.Pitting).HasMaxLength(200);
        builder.Property(x => x.Wound).HasMaxLength(200);
        builder.Property(x => x.Peripheral).HasMaxLength(200);
        builder.Property(x => x.PayRecordBy).HasMaxLength(100);
        builder.Property(x => x.ReferStatus).HasMaxLength(20);
        builder.Property(x => x.MTMService).HasMaxLength(50);
        builder.Property(x => x.ServiceRemark).HasMaxLength(2000);
        builder.Property(x => x.ServiceRef).HasMaxLength(200);
        builder.Property(x => x.TelepharmacyMethod).HasMaxLength(100);
        builder.Property(x => x.RecordMethod).HasMaxLength(100);
        builder.Property(x => x.RecordLocation).HasMaxLength(200);
        builder.Property(x => x.TelepharmacyRemark).HasMaxLength(2000);

        // SmartEnum codes (SmokingValue, CigaretteTypeValue, DrinkingValue, DrinkFrequencyValue)
        builder.Property(x => x.Smoke).HasMaxLength(20);
        builder.Property(x => x.CigaretteType).HasMaxLength(20);
        builder.Property(x => x.Alcohol).HasMaxLength(20);
        builder.Property(x => x.AlcoholFQ).HasMaxLength(20);

        for (var i = 1; i <= 15; i++)
        {
            builder.Property<string?>($"MedicationUsed{i}").HasMaxLength(200);
            builder.Property<string?>($"Frequency{i}").HasMaxLength(100);
        }

        builder.HasIndex(x => x.LocationID);
        builder.HasIndex(x => x.PatientID);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
