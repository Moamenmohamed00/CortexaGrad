using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class MedicationsRepository
    : GenericRepository<Medications>, IMedicationRepository
    {
        public MedicationsRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Medications>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.Medications
                .Where(m => m.AdmissionId == admissionId)
                .ToListAsync();
        }


    }
}
