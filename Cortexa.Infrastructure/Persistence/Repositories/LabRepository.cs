using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class LabRepository : GenericRepository<LabOrder>, ILabRepository
    {
        public LabRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<LabOrder>> GetPendingOrdersAsync()
        {
            return await _context.LabOrders
                .Where(o => !o.Results.Any())
                .Include(o => o.Doctor)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LabOrder>> GetOrdersByAdmissionIdAsync(string admissionId)
        {
            return await _context.LabOrders
                .Where(o => o.AdmissionId == admissionId)
                .Include(o => o.Results)
                .Include(o => o.Doctor)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LabResult>> GetResultsByOrderIdAsync(string orderId)
        {
            return await _context.LabResults
                .Where(r => r.LabOrderId == orderId)
                .OrderByDescending(r => r.SampleDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddResultAsync(LabResult result)
        {
            await _context.LabResults.AddAsync(result);
        }

        public async Task AddLabOrderAsync(LabOrder order)
        {
            await _context.LabOrders.AddAsync(order);
        }
    }
}
