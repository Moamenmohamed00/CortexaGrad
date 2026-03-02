using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class VitalSignsRepository
      : GenericRepository<VitalSigns>, IVitalSignsRepository
    {
        public VitalSignsRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<VitalSigns>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.VitalSigns
                .Where(v => v.AdmissionId == admissionId)
                .OrderByDescending(v => v.RecordedAt)
                .ToListAsync();
        }

  
    }
}
