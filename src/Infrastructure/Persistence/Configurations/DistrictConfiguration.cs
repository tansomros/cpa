using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("Districts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ProvinceId).IsRequired().HasMaxLength(10);

        builder.HasOne(x => x.Province)
            .WithMany(x => x.Districts)
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
