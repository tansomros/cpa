using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class RegisterConfiguration : IEntityTypeConfiguration<Register>
{
    public void Configure(EntityTypeBuilder<Register> builder)
    {
        builder.ToTable("Registers");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.LocationID).HasMaxLength(50);
        builder.Property(x => x.LicenseNo).HasMaxLength(50);
        builder.Property(x => x.LocationName).HasMaxLength(300);
        builder.Property(x => x.LocationName2).HasMaxLength(300);
        builder.Property(x => x.NHSOCode).HasMaxLength(50);
        builder.Property(x => x.LocationType).HasMaxLength(50);
        builder.Property(x => x.LocationGroupID).HasMaxLength(50);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.ProvinceID).HasMaxLength(10);
        builder.Property(x => x.ProvinceName).HasMaxLength(200);
        builder.Property(x => x.ZipCode).HasMaxLength(10);
        builder.Property(x => x.Office_Tel).HasMaxLength(50);
        builder.Property(x => x.Office_Mail).HasMaxLength(200);
        builder.Property(x => x.Office_Hour).HasMaxLength(200);
        builder.Property(x => x.LineID).HasMaxLength(100);
        builder.Property(x => x.Co_Name).HasMaxLength(200);
        builder.Property(x => x.Co_LicenseNo).HasMaxLength(50);
        builder.Property(x => x.Co_Mail).HasMaxLength(200);
        builder.Property(x => x.Co_Tel).HasMaxLength(50);
        builder.Property(x => x.RegisYear).HasMaxLength(20);
        builder.Property(x => x.Lat).HasMaxLength(50);
        builder.Property(x => x.Lng).HasMaxLength(50);

        builder.HasIndex(x => x.LicenseNo);
        builder.HasIndex(x => x.LocationID);

        builder.HasOne<Province>()
            .WithMany()
            .HasForeignKey(x => x.ProvinceID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
