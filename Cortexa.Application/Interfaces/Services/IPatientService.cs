using System.Threading.Tasks;
using Cortexa.Application.Dtos.Actors;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<PatientDto> RegisterPatientAsync(PatientDto dto);
        Task<PatientDto> GetPatientByIdAsync(string id);
        Task<PatientDto> UpdatePatientAsync(PatientDto dto);
    }
}
