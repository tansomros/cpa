using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PrefixConfiguration : IEntityTypeConfiguration<Prefix>
{
    public void Configure(EntityTypeBuilder<Prefix> builder)
    {
        builder.ToTable("Prefixs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}
