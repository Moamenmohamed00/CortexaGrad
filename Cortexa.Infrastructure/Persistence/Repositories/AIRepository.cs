using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class AIRepository : GenericRepository<Alert>, IAIRepository
    {
        public AIRepository(CortexaDbContext context) : base(context) { }

        public IQueryable<Alert> GetAlertsQuery()
        {
            return _context.Alerts
                .AsNoTracking()
                .Include(a => a.Admission)
                .ThenInclude(a => a.Patient);
        }

    }
    
}
