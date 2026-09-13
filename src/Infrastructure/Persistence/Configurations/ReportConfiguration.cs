using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kondongpu.Domain.Entities;

namespace Kondongpu.Infrastructure.Persistence.Configurations;
public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder
            .HasKey(x => x.Id);
        builder.Property(r => r.Wbc).HasColumnType("jsonb");
        builder.Property(r => r.Fbs).HasColumnType("jsonb");
        builder.Property(r => r.Lipid).HasColumnType("jsonb");
        builder.Property(r => r.Renal).HasColumnType("jsonb");
        builder.Property(r => r.Liver).HasColumnType("jsonb");
        builder.Property(r => r.Urine).HasColumnType("jsonb");
        builder.Property(r => r.Stool).HasColumnType("jsonb");
        builder.Property(r => r.OtherLab).HasColumnType("jsonb");

        //builder
        //    .HasMany(e => e.Provinces)
        //    .WithOne(p => p.Checkup)
        //    .HasForeignKey(e => e.Id)
        //    .HasPrincipalKey(p => p.Id)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder
        //    .HasMany(e => e.Labs)
        //    .WithOne(p => p.Report)
        //    .HasForeignKey(p => p.VisitNumber)
        //    .HasPrincipalKey(p => p.VisitNumber)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}
