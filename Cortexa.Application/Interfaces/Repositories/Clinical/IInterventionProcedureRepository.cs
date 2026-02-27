using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface IInterventionProcedureRepository
    : IGenericRepository<InterventionProcedure>
    {
        Task<IReadOnlyList<InterventionProcedure>> GetByAdmissionIdAsync(string admissionId);
    }
}
