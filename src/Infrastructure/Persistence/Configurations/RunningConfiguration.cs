using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class RunningConfiguration : IEntityTypeConfiguration<Running>
{
    public void Configure(EntityTypeBuilder<Running> builder)
    {
        builder.ToTable("Runnings");
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code).HasMaxLength(10);
    }
}
