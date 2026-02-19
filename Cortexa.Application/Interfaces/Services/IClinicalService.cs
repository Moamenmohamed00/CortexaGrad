using System.Threading.Tasks;
using Cortexa.Application.Dtos.Clinical;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IClinicalService
    {
        Task<VitalSignsDto> RecordVitalsAsync(string admissionId, VitalSignsDto vitals);
        Task<MedicationDto> PrescribeMedicationAsync(string admissionId, MedicationDto medication);
        Task<NursingNotesDto> AddNursingNoteAsync(string admissionId, NursingNotesDto note);
        Task<FluidBalanceDto> RecordFluidBalanceAsync(string admissionId, FluidBalanceDto fluid);
        /*Task<CaseHistoryDto> AddCaseHistoryAsync(string admissionId, CaseHistoryDto caseHistory);
         task<InterventionProcedureDto> AddInterventionProcedureAsync(string admissionId, InterventionProcedureDto procedure);*/
    }
}
