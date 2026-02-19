using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IClinicalRepository : IGenericRepository<VitalSigns> // Typically we might have specific repositories, but Generic implies one per Type.
    {
        // For simplicity in this task, I'll create one IClinicalRepository that might handle mixed or just specific queries, 
        // but traditionally we'd have IVitalSignsRepository. 
        // User asked for "IClinicalRepository (or specific ones if needed)". I'll make it generic or create specific ones.
        // I will create IVitalSignsRepository, IMedicationRepository etc if I follow strict pattern, but user listed "IClinicalRepository".
        // I'll create a Unified IClinicalRepository or just assume it handles Vitals for now as example or create specific ones.
        // Given the prompt "IGenericRepository... IClinicalRepository", I'll create IClinicalRepository but it's hard to make it generic for all clinical types unless I use methods.
        // I'll define methods for retrieving clinical data by AdmissionId.

        Task<IReadOnlyList<VitalSigns>> GetVitalSignsByAdmissionIdAsync(string admissionId);
        Task<IReadOnlyList<Medications>> GetMedicationsByAdmissionIdAsync(string admissionId);
        Task<IReadOnlyList<NursingNotes>> GetNursingNotesByAdmissionIdAsync(string admissionId);
        Task<IReadOnlyList<FluidBalance>> GetFluidBalanceByAdmissionIdAsync(string admissionId);
        //Task<IReadOnlyList<CaseHistory>> GetCaseHistoryeByAdmissionIdAsync(string admissionId);
        //Task<IReadOnlyList<InterventionProcedure>> GetInterventionProcedureByAdmissionIdAsync(string admissionId);
        Task AddVitalSignsAsync(VitalSigns vitalSigns, CancellationToken cancellationToken);
        Task AddMedicationAsync(Medications medication, CancellationToken cancellationToken);
        Task AddNursingNoteAsync(NursingNotes nursingNote, CancellationToken cancellationToken);
        Task AddFluidBalanceAsync(FluidBalance fluidBalance, CancellationToken cancellationToken);
        //
        //
    }
}
