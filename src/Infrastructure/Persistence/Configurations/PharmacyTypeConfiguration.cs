using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PharmacyTypeConfiguration : IEntityTypeConfiguration<PharmacyType>
{
    public void Configure(EntityTypeBuilder<PharmacyType> builder)
    {
        builder.ToTable("PharmacyTypes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasIndex(x => x.Code).IsUnique();
    }
}
