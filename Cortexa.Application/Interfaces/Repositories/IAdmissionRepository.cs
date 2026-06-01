using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IAdmissionRepository : IGenericRepository<Admission>
    {
        Task<IReadOnlyList<Admission>> GetActiveAdmissionsAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<Admission>> GetAdmissionsByPatientIdAsync(string patientId, CancellationToken cancellationToken);

        Task<IReadOnlyList<Admission>> GetActiveAdmissionsByPatientIdAsync(string patientId, CancellationToken cancellationToken);

        Task<Admission?> GetByIdWithPatientDataAsync(string admissionId, CancellationToken cancellationToken, bool includeDetails = false);

        Task<IReadOnlyList<Admission>> GetAdmissionsByDateAsync(DateTime date, CancellationToken cancellationToken, bool includeDetails = false);

        Task<IReadOnlyList<Admission>> GetDischargesByDateAsync(DateTime date, CancellationToken cancellationToken, bool includeDetails = false);
    }
}
