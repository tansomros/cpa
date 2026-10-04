using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class PaymentConfigConfiguration : IEntityTypeConfiguration<PaymentConfig>
{
    public void Configure(EntityTypeBuilder<PaymentConfig> builder)
    {
        builder.ToTable("PaymentConfigs");
        builder.HasKey(x => x.itemID);

        builder.Property(x => x.ProvinceID).HasMaxLength(10);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);

        builder.HasOne<Province>()
            .WithMany()
            .HasForeignKey(x => x.ProvinceID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(x => x.PaymentID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
