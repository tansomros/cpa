using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cpa.Domain.Entities;

namespace Cpa.Infrastructure.Persistence.Configurations;

public class SubDistrictConfiguration : IEntityTypeConfiguration<SubDistrict>
{
    public void Configure(EntityTypeBuilder<SubDistrict> builder)
    {
    }
}
