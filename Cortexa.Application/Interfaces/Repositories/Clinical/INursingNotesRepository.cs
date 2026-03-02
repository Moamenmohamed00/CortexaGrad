using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface INursingNotesRepository : IGenericRepository<NursingNotes>
    {
        Task<IReadOnlyList<NursingNotes>> GetByAdmissionIdAsync(string admissionId);
    }
}
