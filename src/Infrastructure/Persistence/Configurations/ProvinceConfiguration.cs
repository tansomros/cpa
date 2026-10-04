using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("Provinces");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(510);
        builder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Region).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ProvinceGroupId).HasMaxLength(20);

        builder.HasOne(x => x.ProvinceGroup)
            .WithMany()
            .HasForeignKey(x => x.ProvinceGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
