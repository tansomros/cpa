using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class LabUOMConfiguration : IEntityTypeConfiguration<LabUOM>
{
    public void Configure(EntityTypeBuilder<LabUOM> builder)
    {
        builder.ToTable("LabUOMs");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.Descriptions).HasMaxLength(100);
    }
}
