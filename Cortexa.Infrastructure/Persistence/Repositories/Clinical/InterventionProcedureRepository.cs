using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class InterventionProcedureRepository
    : GenericRepository<InterventionProcedure>, IInterventionProcedureRepository
    {
        public InterventionProcedureRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<InterventionProcedure>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.InterventionProcedures
                .Where(i => i.AdmissionId == admissionId)
                .ToListAsync();
        }


    }
}
