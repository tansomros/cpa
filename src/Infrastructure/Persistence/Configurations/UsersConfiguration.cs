using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cpa.Domain.Entities;

namespace Cpa.Infrastructure.Persistence.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username).HasMaxLength(50);
        builder.Property(x => x.PasswordHash).HasMaxLength(200);
        builder.Property(x => x.DisplayName).HasMaxLength(100);
        builder.Property(x => x.PositionName).HasMaxLength(100);
    }
}
