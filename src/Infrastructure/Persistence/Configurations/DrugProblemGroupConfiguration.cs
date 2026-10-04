using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class DrugProblemGroupConfiguration : IEntityTypeConfiguration<DrugProblemGroup>
{
    public void Configure(EntityTypeBuilder<DrugProblemGroup> builder)
    {
        builder.ToTable("DrugProblemGroups");
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code).HasMaxLength(20);
        builder.Property(x => x.Descriptions).HasMaxLength(500);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);
    }
}
