using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class MTMBehaviorConfiguration : IEntityTypeConfiguration<MTMBehavior>
{
    public void Configure(EntityTypeBuilder<MTMBehavior> builder)
    {
        builder.ToTable("MTMBehaviors");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.ProblemOther).HasMaxLength(500);
        builder.Property(x => x.Interventions).HasMaxLength(2000);
        builder.Property(x => x.FinalResult).HasMaxLength(500);
        builder.Property(x => x.FinalResultOther).HasMaxLength(500);
        builder.Property(x => x.FatFollow).HasMaxLength(500);
        builder.Property(x => x.TasteFollow).HasMaxLength(500);
        builder.Property(x => x.ResultBegin).HasMaxLength(500);
        builder.Property(x => x.ResultEnd).HasMaxLength(500);
        builder.Property(x => x.Remark).HasMaxLength(2000);
        builder.Property(x => x.isFollow).HasMaxLength(10);

        builder.HasOne<MTM>()
            .WithMany()
            .HasForeignKey(x => x.MTMUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<BehaviorProblemItem>()
            .WithMany()
            .HasForeignKey(x => x.ProblemUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
