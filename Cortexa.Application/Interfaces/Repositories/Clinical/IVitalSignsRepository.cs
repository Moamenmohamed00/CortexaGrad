using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface IVitalSignsRepository : IGenericRepository<VitalSigns>
    {
        Task<IReadOnlyList<VitalSigns>> GetByAdmissionIdAsync(string admissionId);
    }
}
