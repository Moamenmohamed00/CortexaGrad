using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IAdmissionRepository : IGenericRepository<Admission>
    {
        Task<IReadOnlyList<Admission>> GetActiveAdmissionsAsync();
        Task<IReadOnlyList<Admission>> GetAdmissionsByPatientIdAsync(string patientId);
    }
}
