using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable("News");
        builder.HasKey(x => x.NewsID);

        builder.Property(x => x.Title).HasMaxLength(300);
        builder.Property(x => x.NewsType).HasMaxLength(50);
        builder.Property(x => x.LinkPath).HasMaxLength(500);
        builder.Property(x => x.ContentNews).HasColumnType("text");

        builder.HasIndex(x => x.NewsDate);
    }
}
