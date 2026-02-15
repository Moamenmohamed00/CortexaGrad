using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cortexa.Application.Common.Interfaces;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Infrastructure.Identity;

namespace Cortexa.Infrastructure.Persistence
{
    public class CortexaDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;

        public CortexaDbContext(
            DbContextOptions<CortexaDbContext> options,
            IDateTime dateTime,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        // Actors
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Nurse> Nurses => Set<Nurse>();

        // Core
        public DbSet<Admission> Admissions => Set<Admission>();

        // Clinical
        public DbSet<VitalSigns> VitalSigns => Set<VitalSigns>();
        public DbSet<Medications> Medications => Set<Medications>();
        public DbSet<NursingNotes> NursingNotes => Set<NursingNotes>();
        public DbSet<FluidBalance> FluidBalances => Set<FluidBalance>();
        public DbSet<CaseHistory> CaseHistories => Set<CaseHistory>();
        public DbSet<PhysicalExamination> PhysicalExaminations => Set<PhysicalExamination>();
        public DbSet<InterventionProcedure> InterventionProcedures => Set<InterventionProcedure>();

        // Diagnostics
        public DbSet<LabOrder> LabOrders => Set<LabOrder>();
        public DbSet<LabResult> LabResults => Set<LabResult>();
        public DbSet<Imaging> Imagings => Set<Imaging>();
        public DbSet<Culture> Cultures => Set<Culture>();

        // AI
        public DbSet<Alert> Alerts => Set<Alert>();
        public DbSet<AlertOverrideLog> AlertOverrideLogs => Set<AlertOverrideLog>();
        public DbSet<KnowledgeSource> KnowledgeSources => Set<KnowledgeSource>();
        public DbSet<RAGQuery> RAGQueries => Set<RAGQuery>();

        // Infrastructure
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Bed> Beds => Set<Bed>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CortexaDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = _dateTime.Now;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = _dateTime.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
