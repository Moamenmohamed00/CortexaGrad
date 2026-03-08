using System.Text.Json;
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
using Cortexa.Domain.Enums;
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
            var auditEntries = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                // Intercept hard deletes → convert to soft delete
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = _dateTime.Now;
                    entry.Entity.DeletedBy = _currentUserService.UserId;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.EnsureId(entry.Entity.GetType().Name); 
                        entry.Entity.CreatedAt = _dateTime.Now;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = _dateTime.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }

                // Generate audit logs for Clinical entities
                if (IsClinicalEntity(entry.Entity))
                {
                    var auditType = entry.State switch
                    {
                        EntityState.Added => AuditType.Create,
                        EntityState.Modified when entry.Entity.IsDeleted => AuditType.Delete,
                        EntityState.Modified => AuditType.Update,
                        _ => (AuditType?)null
                    };

                    if (auditType.HasValue)
                    {
                        var auditLog = new AuditLog
                        {
                            EntityId = entry.Entity.Id,
                            EntityName = entry.Entity.GetType().Name,
                            Type = auditType.Value,
                            Timestamp = _dateTime.Now,
                            UserId = _currentUserService.UserId
                        };

                        if (entry.State == EntityState.Modified)
                        {
                            var changedProps = entry.Properties
                                .Where(p => p.IsModified)
                                .ToList();

                            auditLog.AffectedColumns = string.Join(", ", changedProps.Select(p => p.Metadata.Name));
                            auditLog.OldValue = JsonSerializer.Serialize(
                                changedProps.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue?.ToString()));
                            auditLog.NewValue = JsonSerializer.Serialize(
                                changedProps.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue?.ToString()));
                        }
                        else if (entry.State == EntityState.Added)
                        {
                            auditLog.NewValue = JsonSerializer.Serialize(
                                entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue?.ToString()));
                        }

                        auditEntries.Add(auditLog);
                    }
                }
            }

            // Add audit logs to context
            if (auditEntries.Any())
            {
                AuditLogs.AddRange(auditEntries);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if the entity belongs to the Clinical namespace
        /// </summary>
 
        private static bool IsClinicalEntity(BaseEntity entity)
        {
            return entity is IAuditableEntity;
        }
    }
}
