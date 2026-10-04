using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PharmacyConfiguration : IEntityTypeConfiguration<Pharmacy>
{
    public void Configure(EntityTypeBuilder<Pharmacy> builder)
    {
        builder.ToTable("Pharmacy");

        builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(300);
        builder.Property(p => p.Name2).HasMaxLength(300);
        builder.Property(p => p.LicenseNo).HasMaxLength(50);
        builder.Property(p => p.NhsoCode).HasMaxLength(50);
        builder.Property(p => p.PharmacyTypeOther).HasMaxLength(200);
        builder.Property(p => p.AddressNo).HasMaxLength(500);
        builder.Property(p => p.ProvinceId).HasMaxLength(10);
        builder.Property(p => p.DistrictId).HasMaxLength(10);
        builder.Property(p => p.SubDistrictId).HasMaxLength(10);
        builder.Property(p => p.ZipCode).HasMaxLength(10);
        builder.Property(p => p.Fda_Province).HasMaxLength(200);
        builder.Property(p => p.Office_Tel).HasMaxLength(50);
        builder.Property(p => p.Office_Fax).HasMaxLength(50);
        builder.Property(p => p.Office_Mail).HasMaxLength(200);
        builder.Property(p => p.LineID).HasMaxLength(100);
        builder.Property(p => p.Co_Name).HasMaxLength(200);
        builder.Property(p => p.Co_Mail).HasMaxLength(200);
        builder.Property(p => p.Co_Tel).HasMaxLength(50);
        builder.Property(p => p.RegisYear).HasMaxLength(20);
        builder.Property(p => p.Lat).HasMaxLength(50);
        builder.Property(p => p.Lng).HasMaxLength(50);

        builder.Property<uint>("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();

        builder.HasIndex(p => p.Code);

        builder.HasOne(p => p.PharmacyGroup)
            .WithMany()
            .HasForeignKey(p => p.PharmacyGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PharmacyType)
            .WithMany()
            .HasForeignKey(p => p.PharmacyTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Province)
            .WithMany()
            .HasForeignKey(p => p.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.District)
            .WithMany()
            .HasForeignKey(p => p.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SubDistrict)
            .WithMany()
            .HasForeignKey(p => p.SubDistrictId)
            .HasPrincipalKey(s => s.SubDistrictId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
