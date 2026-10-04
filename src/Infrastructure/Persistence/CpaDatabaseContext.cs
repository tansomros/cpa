using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Infrastructure.Persistence.Interceptors;
using ServiceRecord = BigLion.CPA.Domain.Entities.Services;

#pragma warning disable CS0618 // ReferenceGroup/ReferenceValue ยังคง DbSet ไว้ แต่ถูกแทนที่ด้วย SmartEnum แล้ว
namespace BigLion.CPA.Infrastructure.Persistence
{
    public class CpaDatabaseContext : DbContext, ICpaDatabaseContext
    {
        private readonly AuditableEntitySaveChangesInterceptors _auditableEntitySaveChangesInterceptors;

        public override DatabaseFacade Database { get; }  
     
        public DbSet<Bank> Banks => Set<Bank>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<SubDistrict> SubDistricts => Set<SubDistrict>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<ProvinceGroup> ProvinceGroups => Set<ProvinceGroup>();
        public DbSet<Prefix> Prefixs => Set<Prefix>();
        public DbSet<RunningConfig> RunningConfigs => Set<RunningConfig>();
        public DbSet<Running> Runnings => Set<Running>();

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRoles> UserRoles => Set<UserRoles>();
        public DbSet<UserLogFile> UserLogFiles => Set<UserLogFile>();

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<PharmacyType> PharmacyTypes => Set<PharmacyType>();
        public DbSet<PharmacyGroup> PharmacyGroups => Set<PharmacyGroup>();
        public DbSet<Pharmacy> Pharmacy => Set<Pharmacy>();
        public DbSet<Pharmacist> Pharmacists => Set<Pharmacist>();
        public DbSet<Register> Registers => Set<Register>();

        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<ServiceRecord> Services => Set<ServiceRecord>();
        public DbSet<Hospital> Hospitals => Set<Hospital>();
        public DbSet<News> News => Set<News>();

        public DbSet<DrugMaster> DrugMasters => Set<DrugMaster>();
        public DbSet<DrugProblemGroup> DrugProblemGroups => Set<DrugProblemGroup>();
        public DbSet<DrugProblemItem> DrugProblemItems => Set<DrugProblemItem>();
        public DbSet<Desease> Deseases => Set<Desease>();
        public DbSet<LabUOM> LabUOMs => Set<LabUOM>();
        public DbSet<LabItem> LabItems => Set<LabItem>();
        public DbSet<LabResult> LabResults => Set<LabResult>();
        public DbSet<BehaviorProblemItem> BehaviorProblemItems => Set<BehaviorProblemItem>();
        public DbSet<BehaviorProblem> BehaviorProblems => Set<BehaviorProblem>();

        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<PaymentConfig> PaymentConfigs => Set<PaymentConfig>();

        public DbSet<MTM> MTMs => Set<MTM>();
        public DbSet<MTMBehavior> MTMBehaviors => Set<MTMBehavior>();
        public DbSet<MTMDesease> MTMDeseases => Set<MTMDesease>();
        public DbSet<MTMDrugProblem> MTMDrugProblems => Set<MTMDrugProblem>();
        public DbSet<MTMDrugRemain> MTMDrugRemains => Set<MTMDrugRemain>();
        public DbSet<MTMRefer> MTMRefers => Set<MTMRefer>();
        public DbSet<Dispense> Dispenses => Set<Dispense>();

        //public DbSet<ReferenceGroup> ReferenceGroups => Set<ReferenceGroup>();
        //public DbSet<ReferenceValue> ReferenceValues => Set<ReferenceValue>();


        public CpaDatabaseContext(
            DbContextOptions<CpaDatabaseContext> options,
            AuditableEntitySaveChangesInterceptors auditableEntitySaveChangesInterceptors)
            : base(options)
        {
            Database = base.Database;
            _auditableEntitySaveChangesInterceptors = auditableEntitySaveChangesInterceptors;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CpaDatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptors);
        }
    }
}
