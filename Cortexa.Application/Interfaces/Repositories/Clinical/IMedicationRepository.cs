using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface IMedicationRepository : IGenericRepository<Medications>
    {
        Task<IReadOnlyList<Medications>> GetByAdmissionIdAsync(string admissionId);

        Task<Medications?> GetLatestByAdmissionIdAsync(string admissionId);
    }
}
