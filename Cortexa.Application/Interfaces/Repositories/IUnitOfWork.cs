using Cortexa.Application.Interfaces.Repositories.Clinical;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository accessors
        IPatientRepository Patients { get; }
        IAdmissionRepository Admissions { get; }
        //IClinicalRepository Clinical { get; }
        IVitalSignsRepository VitalSigns { get; }
        INursingNotesRepository NursingNotes { get; }

        ICaseHistoryRepository CaseHistories { get; }
        IFluidBalanceRepository FluidBalances { get; }
        IInterventionProcedureRepository InterventionProcedures { get; }
        IMedicationRepository Medications { get; }

        IPhysicalExaminationRepository PhysicalExaminations { get; }


        ILabRepository Labs { get; }
        IImagingRepository Imaging { get; }
        IDoctorRepository Doctors { get; }
        IAIRepository AI { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        // managment all transaction on this tables to be synchronized
    }
}
