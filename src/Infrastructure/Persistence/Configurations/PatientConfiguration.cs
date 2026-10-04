using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Prefix).IsRequired().HasMaxLength(50);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.MiddleName).HasMaxLength(200);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Gender).IsRequired().HasMaxLength(20);
        builder.Property(x => x.BirthDate).HasColumnType("date");
        builder.Property(x => x.NationId).HasMaxLength(20);
        builder.Property(x => x.BloodGroup).HasMaxLength(10);
        builder.Property(x => x.AddressNo).HasMaxLength(500);
        builder.Property(x => x.SubDistrictId).HasMaxLength(10);
        builder.Property(x => x.DistrictId).HasMaxLength(10);
        builder.Property(x => x.ProvinceId).HasMaxLength(10);
        builder.Property(x => x.ZipCode).HasMaxLength(10);
        builder.Property(x => x.TelephoneNumber).HasMaxLength(50);
        builder.Property(x => x.DrugAllergy).HasMaxLength(2000);
        builder.Property(x => x.ChronicDisease).HasMaxLength(2000);
        builder.Property(x => x.MainClaim).HasMaxLength(200);
        builder.Property(x => x.Education).HasMaxLength(200);
        builder.Property(x => x.Occupation).HasMaxLength(200);
        builder.Property(x => x.SmokeRemark).HasMaxLength(2000);
        builder.Property(x => x.DrinkRemark).HasMaxLength(2000);

        builder.HasIndex(x => x.NationId);

        builder.HasOne(x => x.Province)
            .WithMany()
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.District)
            .WithMany()
            .HasForeignKey(x => x.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SubDistrict)
            .WithMany()
            .HasForeignKey(x => x.SubDistrictId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
