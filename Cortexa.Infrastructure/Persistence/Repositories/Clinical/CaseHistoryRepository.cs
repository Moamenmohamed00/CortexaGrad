using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class CaseHistoryRepository
    : GenericRepository<CaseHistory>, ICaseHistoryRepository
    {
        public CaseHistoryRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<CaseHistory>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.CaseHistories
                .Where(c => c.AdmissionId == admissionId)
                .ToListAsync();
        }

    
    }
}
