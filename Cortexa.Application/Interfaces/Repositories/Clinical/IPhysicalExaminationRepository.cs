using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface IPhysicalExaminationRepository
    : IGenericRepository<PhysicalExamination>
    {
        Task<IReadOnlyList<PhysicalExamination>> GetByAdmissionIdAsync(string admissionId);
    }
}
