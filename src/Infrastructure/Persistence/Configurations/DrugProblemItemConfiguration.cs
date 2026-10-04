using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class DrugProblemItemConfiguration : IEntityTypeConfiguration<DrugProblemItem>
{
    public void Configure(EntityTypeBuilder<DrugProblemItem> builder)
    {
        builder.ToTable("DrugProblemItems");
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code).HasMaxLength(20);
        builder.Property(x => x.Descriptions).HasMaxLength(500);
        builder.Property(x => x.DrugProblemGroupUID).HasMaxLength(20);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);

        builder.HasOne<DrugProblemGroup>()
            .WithMany()
            .HasForeignKey(x => x.DrugProblemGroupUID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
