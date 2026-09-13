using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BigLion.Domain.Entities;

namespace BigLion.Infrastructure.Persistence.Configurations
{
    public class CareproviderConfiguration : IEntityTypeConfiguration<CareProvider>
    {
        public void Configure(EntityTypeBuilder<CareProvider> builder)
        {

        }
    }
}
