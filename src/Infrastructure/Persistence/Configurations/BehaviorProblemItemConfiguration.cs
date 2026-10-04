using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class BehaviorProblemItemConfiguration : IEntityTypeConfiguration<BehaviorProblemItem>
{
    public void Configure(EntityTypeBuilder<BehaviorProblemItem> builder)
    {
        builder.ToTable("BehaviorProblemItems");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.Descriptions).HasMaxLength(500);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);
    }
}
