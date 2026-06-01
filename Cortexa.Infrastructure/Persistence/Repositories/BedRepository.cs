using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Cortexa.Domain.Enums;
namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class BedRepository : GenericRepository<Bed>, IBedRepository
    {
        public BedRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Bed>> GetOccupiedBedsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Beds.Where(b => b.Status == BedStatus.Occupied).ToListAsync(cancellationToken);
        }
    }
    
}
