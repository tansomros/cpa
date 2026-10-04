using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class MTMDrugProblemConfiguration : IEntityTypeConfiguration<MTMDrugProblem>
{
    public void Configure(EntityTypeBuilder<MTMDrugProblem> builder)
    {
        builder.ToTable("MTMDrugProblems");
        builder.HasKey(x => x.UID);

        builder.Property(x => x.ServiceTypeID).HasMaxLength(20);
        builder.Property(x => x.ProblemGroupUID).HasMaxLength(20);
        builder.Property(x => x.ProblemUID).HasMaxLength(20);
        builder.Property(x => x.ProblemOther).HasMaxLength(500);
        builder.Property(x => x.Interventions).HasMaxLength(2000);
        builder.Property(x => x.FinalResult).HasMaxLength(500);
        builder.Property(x => x.FinalResultOther).HasMaxLength(500);
        builder.Property(x => x.CUser).HasMaxLength(100);
        builder.Property(x => x.MUser).HasMaxLength(100);
        builder.Property(x => x.TMTID).HasMaxLength(50);

        builder.HasOne<MTM>()
            .WithMany()
            .HasForeignKey(x => x.MTMUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ServiceType>()
            .WithMany()
            .HasForeignKey(x => x.ServiceTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DrugProblemGroup>()
            .WithMany()
            .HasForeignKey(x => x.ProblemGroupUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DrugProblemItem>()
            .WithMany()
            .HasForeignKey(x => x.ProblemUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DrugMaster>()
            .WithMany()
            .HasForeignKey(x => x.DrugUID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
