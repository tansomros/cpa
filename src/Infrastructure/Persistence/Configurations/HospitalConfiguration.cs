using BigLion.CPA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigLion.CPA.Infrastructure.Persistence.Configurations;

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.ToTable("Hospitals");
        builder.HasKey(x => x.HospitalUID);

        builder.Property(x => x.HospitalName).HasMaxLength(300);
        builder.Property(x => x.DepartmentName).HasMaxLength(200);
        builder.Property(x => x.Office_hours).HasMaxLength(200);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.ProvinceID).HasMaxLength(10);
        builder.Property(x => x.ProvinceName).HasMaxLength(200);
        builder.Property(x => x.ZipCode).HasMaxLength(10);
        builder.Property(x => x.Office_Tel).HasMaxLength(50);
        builder.Property(x => x.Office_Fax).HasMaxLength(50);
        builder.Property(x => x.Co_Name).HasMaxLength(200);
        builder.Property(x => x.Co_Position).HasMaxLength(200);
        builder.Property(x => x.Co_Mail).HasMaxLength(200);
        builder.Property(x => x.Co_Tel).HasMaxLength(50);
        builder.Property(x => x.StatusFlag).HasMaxLength(10);
        builder.Property(x => x.Bill_Name).HasMaxLength(300);
        builder.Property(x => x.WorkDayDesc).HasMaxLength(200);
        builder.Property(x => x.WorkTimeDesc).HasMaxLength(200);
        builder.Property(x => x.ConfirmHold).HasMaxLength(10);
        builder.Property(x => x.BranchRemark).HasMaxLength(500);
        builder.Property(x => x.WorkList).HasColumnType("text");
        builder.Property(x => x.WorkSpec).HasColumnType("text");
        builder.Property(x => x.WorkTop).HasMaxLength(500);
        builder.Property(x => x.Remark).HasMaxLength(2000);
        builder.Property(x => x.Informant).HasMaxLength(200);
        builder.Property(x => x.InfoPosition).HasMaxLength(200);
        builder.Property(x => x.InfoDate).HasMaxLength(50);
        builder.Property(x => x.LetterTo).HasMaxLength(300);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.ZoneID).HasMaxLength(20);
        builder.Property(x => x.OfficeID).HasMaxLength(50);
        builder.Property(x => x.Website).HasMaxLength(300);
        builder.Property(x => x.Facebook).HasMaxLength(300);
        builder.Property(x => x.Lat).HasMaxLength(50);
        builder.Property(x => x.Lng).HasMaxLength(50);
        builder.Property(x => x.MUser).HasMaxLength(100);

        builder.HasIndex(x => x.HospitalName);

        builder.HasOne<Province>()
            .WithMany()
            .HasForeignKey(x => x.ProvinceID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
