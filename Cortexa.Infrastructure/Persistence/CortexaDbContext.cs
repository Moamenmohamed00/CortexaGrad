using Cortexa.Application.Common.Interfaces;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using Cortexa.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using Cortexa.Domain.Entities.StaffSchedule;

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

        // Staff Schedule
        public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();

        public DbSet<NurseSchedule> NurseSchedules => Set<NurseSchedule>();

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
        //Audit
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CortexaDbContext).Assembly);

            // Global query filter: automatically exclude soft-deleted entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                    var condition = System.Linq.Expressions.Expression.Equal(property, falseConstant);
                    var lambda = System.Linq.Expressions.Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. تثبيت الوقت لضمان تطابق التوقيت في كل الجداول
            var currentTime = _dateTime.Now;
            var userId = _currentUserService.UserId;

            // 2. تحديث الداتا الأساسية (Soft Delete & Metadata)
            ApplySoftDelete(currentTime, userId);
            ApplyMetadata(currentTime, userId);

            // 3. تحضير الـ Audit Logs (بما أن الـ ID يتولد يدوياً، لا داعي لانتظار الـ Database)
            var auditEntries = PrepareAuditEntries(currentTime, userId);

            if (auditEntries.Any())
            {
                AuditLogs.AddRange(auditEntries);
            }

            // 4. حفظ كل شيء في Call واحدة لقاعدة البيانات (Atomic Transaction)
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplySoftDelete(DateTime now, string userId)
        {
            var deletedEntries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in deletedEntries)
            {
                entry.State = EntityState.Modified; // تحويل الحذف المسح لـ Update
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = now;
                entry.Entity.DeletedBy = userId;
            }
        }

        private void ApplyMetadata(DateTime now, string userId)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedAt = now;
                    entry.Entity.LastModifiedBy = userId;
                }
            }
        }

        private List<AuditLog> PrepareAuditEntries(DateTime now, string userId)
        {
            var auditEntries = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                if (!ShouldAuditEntity(entry.Entity))
                    continue;

                var auditType = entry.State switch
                {
                    EntityState.Added => AuditType.Create,
                    EntityState.Modified when entry.Entity.IsDeleted => AuditType.Delete, // حالة الـ Soft Delete
                    EntityState.Modified => AuditType.Update,
                    _ => (AuditType?)null
                };

                if (auditType == null) continue;

                var auditLog = new AuditLog
                {
                    EntityId = entry.Entity.Id,
                    EntityName = entry.Entity.GetType().Name,
                    Type = auditType.Value,
                    Timestamp = now,
                    UserId = userId
                };

                // منطق الـ Serialization (يفضل عمله فقط عند الحاجة لتوفير الأداء)
                if (entry.State == EntityState.Added)
                {
                    auditLog.NewValue = SerializeProperties(entry.Properties, isCurrent: true);
                }
                else if (entry.State == EntityState.Modified)
                {
                    var changedProps = entry.Properties.Where(p => p.IsModified).ToList();
                    if (!changedProps.Any()) continue;

                    auditLog.AffectedColumns = string.Join(", ", changedProps.Select(p => p.Metadata.Name));
                    auditLog.OldValue = SerializeProperties(changedProps, isCurrent: false);
                    auditLog.NewValue = SerializeProperties(changedProps, isCurrent: true);
                }

                auditEntries.Add(auditLog);
            }
            return auditEntries;
        }
        private static bool ShouldAuditEntity(BaseEntity entity)
        {
            return entity is IAuditableEntity;
        }

        private string SerializeProperties(IEnumerable<PropertyEntry> properties, bool isCurrent)
        {
            var dict = properties.ToDictionary(
                p => p.Metadata.Name,
                p => (isCurrent ? p.CurrentValue : p.OriginalValue)?.ToString()
            );
            return JsonSerializer.Serialize(dict);
        }
    }
}
