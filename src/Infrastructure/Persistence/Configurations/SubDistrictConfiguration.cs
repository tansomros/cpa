using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class SubDistrictConfiguration : IEntityTypeConfiguration<SubDistrict>
{
    public void Configure(EntityTypeBuilder<SubDistrict> builder)
    {
        builder.ToTable("SubDistricts");
        builder.HasKey(x => x.SubDistrictId);

        builder.Property(x => x.SubDistrictId).HasMaxLength(10);
        builder.Property(x => x.ProvinceId).IsRequired().HasMaxLength(10);
        builder.Property(x => x.DistrictId).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ZipCode).IsRequired().HasMaxLength(10);

        builder.HasOne(x => x.District)
            .WithMany(x => x.SubDistricts)
            .HasForeignKey(x => x.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ZipCode);
    }
}
