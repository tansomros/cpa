using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
{
    public void Configure(EntityTypeBuilder<ServiceType> builder)
    {
        builder.ToTable("ServiceTypes");
        builder.HasKey(x => x.ServiceTypeID);

        builder.Property(x => x.ServiceTypeID).HasMaxLength(20);
        builder.Property(x => x.ServiceName).HasMaxLength(200);
        builder.Property(x => x.Descriptions).HasMaxLength(1000);
        builder.Property(x => x.Status).HasMaxLength(10);
    }
}
