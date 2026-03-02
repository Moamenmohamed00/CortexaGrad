using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface ICaseHistoryRepository : IGenericRepository<CaseHistory>
    {
        Task<IReadOnlyList<CaseHistory>> GetByAdmissionIdAsync(string admissionId);
    }
}
