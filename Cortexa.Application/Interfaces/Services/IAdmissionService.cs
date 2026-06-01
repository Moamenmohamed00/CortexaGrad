using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Models.Dashboard;
using System.Threading.Tasks;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IAdmissionService
    {
        Task TransferPatientAsync(string admissionId, string newBedId, CancellationToken cancellationToken = default);
        Task<string> GetPatientNameByAdmissionIdAsync(string admissionId, CancellationToken cancellationToken = default);
        Task<HospitalOperationsModel> GetHospitalOperationsAsync(CancellationToken cancellationToken = default);
    }
}
