using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Diagnostics;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IImagingRepository : IGenericRepository<Imaging>
    {
        Task<IReadOnlyList<Imaging>> GetImagingByAdmissionIdAsync(string admissionId);
        Task<IReadOnlyList<Imaging>> GetImagingByPatientIdAsync(string patientId); // If needed, but usually by Admission
        Task AddImagingAsync(Imaging imaging);
    }
}
