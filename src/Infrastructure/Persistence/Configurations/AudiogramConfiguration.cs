using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BigLion.Domain.Entities;

namespace BigLion.Infrastructure.Persistence.Configurations
{
    public class AudiogramConfiguration : IEntityTypeConfiguration<Audiogram>
    {
        public void Configure(EntityTypeBuilder<Audiogram> builder)
        {
            builder
                .Property(x => x.LeftNote)
                .IsRequired(true);

            builder
                .Property(d => d.RightNote)
                .IsRequired(true);
        }
    }
}
