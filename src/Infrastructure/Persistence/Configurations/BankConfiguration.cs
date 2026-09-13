using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cpa.Domain.Entities;

namespace Cpa.Infrastructure.Persistence.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(10);
        builder.Property(x => x.Name).HasMaxLength(100);
    }
}
