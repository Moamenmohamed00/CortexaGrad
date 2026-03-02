using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class PhysicalExaminationRepository
    : GenericRepository<PhysicalExamination>, IPhysicalExaminationRepository
    {
        public PhysicalExaminationRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<PhysicalExamination>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.PhysicalExaminations
                .Where(p => p.AdmissionId == admissionId)
                .ToListAsync();
        }


    }
}
