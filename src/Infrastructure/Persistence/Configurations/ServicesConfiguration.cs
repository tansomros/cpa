using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceRecord = BigLion.CPA.Domain.Entities.Services;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class ServicesConfiguration : IEntityTypeConfiguration<ServiceRecord>
{
    public void Configure(EntityTypeBuilder<ServiceRecord> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(x => x.itemID);

        builder.Property(x => x.LocationID).HasMaxLength(50);
        builder.Property(x => x.ServiceTypeID).IsRequired().HasMaxLength(20);
        builder.Property(x => x.CustName).HasMaxLength(300);
        builder.Property(x => x.Gender).HasMaxLength(20);
        builder.Property(x => x.BirthDate).HasMaxLength(20);
        builder.Property(x => x.CardID).HasMaxLength(20);
        builder.Property(x => x.Telephone).HasMaxLength(50);
        builder.Property(x => x.Mobile).HasMaxLength(50);
        builder.Property(x => x.AddressType).HasMaxLength(50);
        builder.Property(x => x.AddressNo).HasMaxLength(500);
        builder.Property(x => x.Road).HasMaxLength(200);
        builder.Property(x => x.District).HasMaxLength(200);
        builder.Property(x => x.City).HasMaxLength(200);
        builder.Property(x => x.ProvinceID).HasMaxLength(10);
        builder.Property(x => x.ProvinceName).HasMaxLength(200);
        builder.Property(x => x.MainClaim).HasMaxLength(200);
        builder.Property(x => x.UpdBy).HasMaxLength(100);
        builder.Property(x => x.ChildName).HasMaxLength(300);
        builder.Property(x => x.ChildBirthDate).HasMaxLength(20);
        builder.Property(x => x.VaccineComplete).HasMaxLength(10);
        builder.Property(x => x.EducateName).HasMaxLength(300);
        builder.Property(x => x.isPapSmear).HasMaxLength(10);
        builder.Property(x => x.HospitalName).HasMaxLength(300);
        builder.Property(x => x.DateCheck).HasMaxLength(20);
        builder.Property(x => x.MedicinceDesc).HasMaxLength(2000);
        builder.Property(x => x.DocFile).HasMaxLength(500);
        builder.Property(x => x.Remark).HasMaxLength(2000);
        builder.Property(x => x.SubProblem9).HasMaxLength(500);
        builder.Property(x => x.SubProblem10).HasMaxLength(500);
        builder.Property(x => x.ProblemRemark).HasMaxLength(2000);
        builder.Property(x => x.EducateRemark).HasMaxLength(2000);
        builder.Property(x => x.ProbMed1).HasMaxLength(500);
        builder.Property(x => x.ProbPro1).HasMaxLength(500);
        builder.Property(x => x.ProbMed2).HasMaxLength(500);
        builder.Property(x => x.ProbPro2).HasMaxLength(500);
        builder.Property(x => x.ProbMed3).HasMaxLength(500);
        builder.Property(x => x.ProbPro3).HasMaxLength(500);
        builder.Property(x => x.EduMed1).HasMaxLength(500);
        builder.Property(x => x.EduPro1).HasMaxLength(500);
        builder.Property(x => x.EduMed2).HasMaxLength(500);
        builder.Property(x => x.EduPro2).HasMaxLength(500);
        builder.Property(x => x.EduMed3).HasMaxLength(500);
        builder.Property(x => x.EduPro3).HasMaxLength(500);
        builder.Property(x => x.vct_Follow1).HasMaxLength(200);
        builder.Property(x => x.vct_Follow2).HasMaxLength(200);
        builder.Property(x => x.vct_FollowDate).HasMaxLength(20);
        builder.Property(x => x.ServicePlan).HasMaxLength(500);
        builder.Property(x => x.InvoiceNo).HasMaxLength(50);
        builder.Property(x => x.Follow_Channel).HasMaxLength(100);
        builder.Property(x => x.NextDate).HasMaxLength(20);
        builder.Property(x => x.FollowDateSave).HasMaxLength(20);
        builder.Property(x => x.IsAbNormal).HasMaxLength(10);
        builder.Property(x => x.AbNormalRemark).HasMaxLength(2000);
        builder.Property(x => x.ChildGender).HasMaxLength(20);
        builder.Property(x => x.CreateBy).HasMaxLength(100);
        builder.Property(x => x.ActiveStatus).HasMaxLength(10);
        builder.Property(x => x.PatientFrom).HasMaxLength(100);
        builder.Property(x => x.isNotResponse).HasMaxLength(10);
        builder.Property(x => x.OtherCause).HasMaxLength(500);
        builder.Property(x => x.PayRecordBy).HasMaxLength(100);

        builder.HasIndex(x => x.CardID);
        builder.HasIndex(x => x.LocationID);
        builder.HasIndex(x => x.PatientID);

        builder.HasOne<ServiceType>()
            .WithMany()
            .HasForeignKey(x => x.ServiceTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Province>()
            .WithMany()
            .HasForeignKey(x => x.ProvinceID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
