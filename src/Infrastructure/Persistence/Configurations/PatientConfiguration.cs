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

        builder.Property(x => x.ForeName).HasMaxLength(200);
        builder.Property(x => x.Surname).HasMaxLength(200);
        builder.Property(x => x.Gender).HasMaxLength(20);
        builder.Property(x => x.BirthDate).HasColumnType("date");
        builder.Property(x => x.CardId).HasMaxLength(20);
        builder.HasIndex(x => x.CardId);

        builder.Property(x => x.Telephone).HasMaxLength(50); 
        builder.Property(x => x.TimeContact).HasMaxLength(100);

        builder.Property(x => x.AddressType).HasMaxLength(50);
        builder.Property(x => x.AddressNo).HasMaxLength(500);
        builder.Property(x => x.Road).HasMaxLength(200);
        builder.Property(x => x.SubDistrictId).HasMaxLength(10);
        builder.Property(x => x.DistrictId).HasMaxLength(200);
        builder.Property(x => x.ProvinceId).HasMaxLength(10); 
        builder.Property(x => x.ZipCode).HasMaxLength(10);

        builder.Property(x => x.MainClaim).HasMaxLength(200);
        builder.Property(x => x.Education).HasMaxLength(200);
        builder.Property(x => x.Occupation).HasMaxLength(200);
        builder.Property(x => x.DrugAllergy).HasMaxLength(2000);
        builder.Property(x => x.SmokingRemark).HasMaxLength(2000);

        builder.HasOne(x => x.Province)
            .WithMany()
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.District)
            .WithMany()
            .HasForeignKey(x => x.SubDistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.SubDistrict);

        builder.Property(x => x.IsAllergy)
    .HasColumnName("isAllergy")
    .HasConversion(
        value => value == true ? "Y" : "N",
        value => value == "Y");

        builder.Property(x => x.IsSmoke)
            .HasColumnName("isSmoke")
            .HasConversion(
                value => value == true ? "Y" : "N",
                value => value == "Y");

        builder.Property(x => x.SmokingQuit)
            .HasColumnName("SmokingQuit")
            .HasConversion(
                value => value == true ? "Y" : "N",
                value => value == "Y");
    }
}
