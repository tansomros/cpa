using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BigLion.Domain.Entities;

namespace BigLion.Infrastructure.Persistence.Configurations;
public class RecommendationTemplateConfiguration : IEntityTypeConfiguration<RecommendationTemplate>
{
    public void Configure(EntityTypeBuilder<RecommendationTemplate> builder)
    {
        builder
            .Property(x => x.Text)
            .HasColumnType("text")
            .IsRequired(true);
    }
}
