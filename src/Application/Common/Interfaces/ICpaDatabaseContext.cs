using Microsoft.EntityFrameworkCore.Infrastructure;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Common.Interfaces
{
    public interface ICpaDatabaseContext
    {
        DatabaseFacade Database { get; }  
        //DbSet<Bank> Banks { get; }    
        DbSet<District> Districts { get; }
        DbSet<Patient> Patients { get; }
        DbSet<Pharmacist> Pharmacists{ get; }
        DbSet<Pharmacy> Pharmacys { get; }
        DbSet<PharmacyGroup> PharmacyGroups{ get; }
        DbSet<PharmacyType> PharmacyTypes { get; }
        DbSet<Prefix> Prefixs { get; }
        DbSet<Province> Provinces { get; }
        DbSet<ProvinceGroup> ProvinceGroups { get; }
        //DbSet<ReferenceGroup> ReferenceGroups { get; }
        //DbSet<ReferenceValue> ReferenceValues { get; }
        DbSet<Role> Roles { get; }     
        DbSet<Running> Runnings { get; }
        DbSet<RunningConfig> RunningConfigs { get; }
        DbSet<SubDistrict> SubDistricts { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
