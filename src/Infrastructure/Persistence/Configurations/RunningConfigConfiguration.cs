using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cpa.Domain.Entities;

namespace Cpa.Infrastructure.Persistence.Configurations;

public class RunningConfigConfiguration : IEntityTypeConfiguration<RunningConfig>
{
    public void Configure(EntityTypeBuilder<RunningConfig> builder)
    {

        builder.HasKey(x => x.Code);
        builder.Property(x => x.Code).HasMaxLength(20);
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.IsCode).HasMaxLength(1);
        builder.Property(x => x.IsRef).HasMaxLength(1);
        builder.Property(x => x.TemplateCode).HasMaxLength(100);
    }
}
