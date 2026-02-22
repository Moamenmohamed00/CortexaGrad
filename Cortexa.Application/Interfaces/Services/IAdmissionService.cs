using System.Threading.Tasks;
using Cortexa.Application.Dtos.Core;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IAdmissionService
    {
        Task<AdmissionDto> AdmitPatientAsync(AdmissionDto dto);
        Task DischargePatientAsync(string admissionId);
        Task TransferPatientAsync(string admissionId, string newBedId);
        Task<AdmissionDto?> GetActiveAdmissionForPatientAsync(string patientId);
    }
}
