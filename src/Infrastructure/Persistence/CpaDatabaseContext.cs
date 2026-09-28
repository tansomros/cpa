using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;
using Cpa.Infrastructure.Persistence.Interceptors;

#pragma warning disable CS0618 // ReferenceGroup/ReferenceValue ยังคง DbSet ไว้ แต่ถูกแทนที่ด้วย SmartEnum แล้ว
namespace Cpa.Infrastructure.Persistence
{
    public class CpaDatabaseContext : DbContext, ICpaDatabaseContext
    {
        private readonly AuditableEntitySaveChangesInterceptors _auditableEntitySaveChangesInterceptors;

        public override DatabaseFacade Database { get; }  
     
        //public DbSet<Bank> Banks => Set<Bank>();
       public DbSet<District> Districts => Set<District>();
        public DbSet<SubDistrict> SubDistricts => Set<SubDistrict>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<ProvinceGroup> ProvinceGroups => Set<ProvinceGroup>();
        public DbSet<Prefix> Prefixs => Set<Prefix>();
        public   DbSet<RunningConfig> RunningConfigs => Set<RunningConfig>();
        public DbSet<Running> Runnings => Set<Running>();
      
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<PharmacyType> PharmacyTypes => Set<PharmacyType>();
        public DbSet<PharmacyGroup> PharmacyGroups => Set<PharmacyGroup>();
        public DbSet<Pharmacy> Pharmacys => Set<Pharmacy>();
        public DbSet<Pharmacist> Pharmacists => Set<Pharmacist>();

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
