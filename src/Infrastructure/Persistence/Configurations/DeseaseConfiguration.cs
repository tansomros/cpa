using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class DeseaseConfiguration : IEntityTypeConfiguration<Desease>
{
    public void Configure(EntityTypeBuilder<Desease> builder)
    {
        builder.ToTable("Deseases");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.Code).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(300);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.IsChapter).HasMaxLength(1);
        builder.Property(x => x.ICD).HasMaxLength(20);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);
        builder.Property(x => x.isICD).HasMaxLength(10);

        builder.HasIndex(x => x.Code);
        builder.HasIndex(x => x.ICD);

        builder.HasOne<Desease>()
            .WithMany()
            .HasForeignKey(x => x.ParentUID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
