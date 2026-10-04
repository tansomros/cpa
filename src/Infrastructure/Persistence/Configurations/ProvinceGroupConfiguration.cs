using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class ProvinceGroupConfiguration : IEntityTypeConfiguration<ProvinceGroup>
{
    public void Configure(EntityTypeBuilder<ProvinceGroup> builder)
    {
        builder.ToTable("ProvinceGroups");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(20);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
    }
}
