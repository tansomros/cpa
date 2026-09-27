using Microsoft.EntityFrameworkCore.Infrastructure;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Common.Interfaces
{
    public interface ICpaDatabaseContext
    {
        DatabaseFacade Database { get; }  
        //DbSet<Bank> Banks { get; }    
        DbSet<Prefix> Prefixs { get; }
        DbSet<Patient> Patients { get; }
        DbSet<District> Districts { get; }
        DbSet<SubDistrict> SubDistricts { get; }
        DbSet<Province> Provinces { get; }
        //DbSet<ReferenceGroup> ReferenceGroups { get; }
        //DbSet<ReferenceValue> ReferenceValues { get; }
        DbSet<RunningConfig> RunningConfigs { get; }
        DbSet<Running> Runnings { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<PharmacyType> PharmacyTypes { get; }
        DbSet<PharmacyGroup> PharmacyGroups{ get; }
        DbSet<Pharmacy> Pharmacys { get; }
        DbSet<Pharmacist> Pharmacists{ get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
