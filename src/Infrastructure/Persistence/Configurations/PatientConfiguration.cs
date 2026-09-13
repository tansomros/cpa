using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BigLion.Domain.Entities;

namespace BigLion.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(e => e.BirthDate).HasColumnType("date");

        // Patient -> Province (via ProvinceId -> Province.ProvinceId)
        builder
            .HasOne(p => p.Province)
            .WithMany()                          // no collection nav on Province
            .HasForeignKey(p => p.ProvinceId)    // FK in Patient
            .HasPrincipalKey(prov => prov.ProvinceId)  // custom key in Province
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Patient -> District (via DistrictId -> District.DistrictId)
        builder
            .HasOne(p => p.District)
            .WithMany()
            .HasForeignKey(p => p.DistrictId)
            .HasPrincipalKey(d => d.DistrictId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Patient -> SubDistrict (via SubDistrictId -> SubDistrict.SubDistrictId)
        builder
            .HasOne(p => p.SubDistrict)
            .WithMany()
            .HasForeignKey(p => p.SubDistrictId)
            .HasPrincipalKey(sd => sd.SubDistrictId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        /** ตัวอย่างการดึงข้อมูลใน patient
        var patient = await _context.Patients
            .Include(p => p.Province)
            .Include(p => p.District)
            .Include(p => p.SubDistrict)
            .FirstOrDefaultAsync(p => p.Id == patientId);
        */

        // Access:
        // patient.Province?.Name
        // patient.District?.Name  
        // patient.SubDistrict?.Name
    }
}
