using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.AI;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IAIRepository : IGenericRepository<Alert>
    {
        Task<IReadOnlyList<Alert>> GetAlertsByPatientIdAsync(string patientId);
        Task<IReadOnlyList<Alert>> GetAlertsByAdmissionIdAsync(string admissionId);
    }
}
