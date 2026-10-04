using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class UserLogFileConfiguration : IEntityTypeConfiguration<UserLogFile>
{
    public void Configure(EntityTypeBuilder<UserLogFile> builder)
    {
        builder.ToTable("UserLogFiles");
        builder.HasKey(x => x.LogID);

        builder.Property(x => x.Act_Type).HasMaxLength(50);
        builder.Property(x => x.DB_Effective).HasMaxLength(100);
        builder.Property(x => x.Descrp).HasMaxLength(2000);
        builder.Property(x => x.Remark).HasMaxLength(2000);

        builder.HasIndex(x => x.UserID);
        builder.HasIndex(x => x.Work_Date);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
