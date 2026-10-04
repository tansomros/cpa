using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");
        builder.HasKey(x => x.PaymentID);

        builder.Property(x => x.PaymentName).HasMaxLength(200);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);
        builder.Property(x => x.ServiceTypeID).HasMaxLength(20);

        builder.HasOne<ServiceType>()
            .WithMany()
            .HasForeignKey(x => x.ServiceTypeID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
