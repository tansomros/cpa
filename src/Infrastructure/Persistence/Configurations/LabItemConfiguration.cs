using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class LabItemConfiguration : IEntityTypeConfiguration<LabItem>
{
    public void Configure(EntityTypeBuilder<LabItem> builder)
    {
        builder.ToTable("LabItems");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.Name).HasMaxLength(300);
        builder.Property(x => x.AliasName).HasMaxLength(300);
        builder.Property(x => x.NormalRange).HasMaxLength(200);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);

        builder.HasIndex(x => x.Name);

        builder.HasOne<LabUOM>()
            .WithMany()
            .HasForeignKey(x => x.UOMUID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
