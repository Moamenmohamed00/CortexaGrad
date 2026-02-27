using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories.Clinical
{
    public class FluidBalanceRepository
    : GenericRepository<FluidBalance>, IFluidBalanceRepository
    {
        public FluidBalanceRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<FluidBalance>> GetByAdmissionIdAsync(string admissionId)
        {
            return await _context.FluidBalances
                .Where(f => f.AdmissionId == admissionId)
                .OrderByDescending(f => f.RecordedAt)
                .ToListAsync();
        }


    }
}
