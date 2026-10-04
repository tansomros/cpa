using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BigLion.CPA.Domain.Entities;

#pragma warning disable CS0618
namespace BigLion.CPA.Application.Common.Interfaces
{
    public interface ICpaDatabaseContext
    {
        DatabaseFacade Database { get; }
        DbSet<Bank> Banks { get; }
        DbSet<BehaviorProblem> BehaviorProblems { get; }
        DbSet<BehaviorProblemItem> BehaviorProblemItems { get; }
        DbSet<Desease> Deseases { get; }
        DbSet<Dispense> Dispenses { get; }
        DbSet<District> Districts { get; }
        DbSet<DrugMaster> DrugMasters { get; }
        DbSet<DrugProblemGroup> DrugProblemGroups { get; }
        DbSet<DrugProblemItem> DrugProblemItems { get; }
        DbSet<Hospital> Hospitals { get; }
        DbSet<LabItem> LabItems { get; }
        DbSet<LabResult> LabResults { get; }
        DbSet<LabUOM> LabUOMs { get; }
        DbSet<MTM> MTMs { get; }
        DbSet<MTMBehavior> MTMBehaviors { get; }
        DbSet<MTMDesease> MTMDeseases { get; }
        DbSet<MTMDrugProblem> MTMDrugProblems { get; }
        DbSet<MTMDrugRemain> MTMDrugRemains { get; }
        DbSet<MTMRefer> MTMRefers { get; }
        DbSet<News> News { get; }
        DbSet<Patient> Patients { get; }
        DbSet<PaymentConfig> PaymentConfigs { get; }
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<Pharmacist> Pharmacists { get; }
        DbSet<Pharmacy> Pharmacy { get; }
        DbSet<PharmacyGroup> PharmacyGroups { get; }
        DbSet<PharmacyType> PharmacyTypes { get; }
        DbSet<Prefix> Prefixs { get; }
        DbSet<Province> Provinces { get; }
        DbSet<ProvinceGroup> ProvinceGroups { get; }
        DbSet<Register> Registers { get; }
        DbSet<Role> Roles { get; }
        DbSet<Running> Runnings { get; }
        DbSet<RunningConfig> RunningConfigs { get; }
        DbSet<Services> Services { get; }
        DbSet<ServiceType> ServiceTypes { get; }
        DbSet<SubDistrict> SubDistricts { get; }
        DbSet<User> Users { get; }
        DbSet<UserLogFile> UserLogFiles { get; }
        DbSet<UserRoles> UserRoles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
