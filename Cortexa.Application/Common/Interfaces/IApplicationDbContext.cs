using Microsoft.EntityFrameworkCore;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Entities.Infrastructure;

namespace Cortexa.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        // Actors
        DbSet<Patient> Patients { get; }
        DbSet<Doctor> Doctors { get; }
        DbSet<Nurse> Nurses { get; }

        // Core
        DbSet<Admission> Admissions { get; }

        // Clinical
        DbSet<VitalSigns> VitalSigns { get; }
        DbSet<Medications> Medications { get; }
        DbSet<NursingNotes> NursingNotes { get; }
        DbSet<FluidBalance> FluidBalances { get; }
        DbSet<CaseHistory> CaseHistories { get; }
        DbSet<PhysicalExamination> PhysicalExaminations { get; }
        DbSet<InterventionProcedure> InterventionProcedures { get; }

        // Diagnostics
        DbSet<LabOrder> LabOrders { get; }
        DbSet<LabResult> LabResults { get; }
        DbSet<Imaging> Imagings { get; }
        DbSet<Culture> Cultures { get; }

        // AI
        DbSet<Alert> Alerts { get; }
        DbSet<AlertOverrideLog> AlertOverrideLogs { get; }
        DbSet<KnowledgeSource> KnowledgeSources { get; }
        DbSet<RAGQuery> RAGQueries { get; }

        // Infrastructure
        DbSet<Room> Rooms { get; }
        DbSet<Bed> Beds { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
