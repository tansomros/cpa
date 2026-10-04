using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class RunningConfigConfiguration : IEntityTypeConfiguration<RunningConfig>
{
    public void Configure(EntityTypeBuilder<RunningConfig> builder)
    {
        builder.ToTable("RunningConfigs");
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code).HasMaxLength(20);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.TemplateCode).HasMaxLength(100);
    }
}
