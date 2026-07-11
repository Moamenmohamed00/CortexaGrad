using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class NursingNotesRepository
    : GenericRepository<NursingNotes>, INursingNotesRepository
    {
        public NursingNotesRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<NursingNotes>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.NursingNotes
                .Where(n => n.AdmissionId == admissionId)
                .Include(n => n.Nurse) 
                .OrderByDescending(n => n.NoteDateTime)
                .ToListAsync();
        }

        public async Task<NursingNotes?> GetLatestByAdmissionIdAsync(string admissionId)
        {
            return await _context.NursingNotes
                .Where(n => n.AdmissionId == admissionId)
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
