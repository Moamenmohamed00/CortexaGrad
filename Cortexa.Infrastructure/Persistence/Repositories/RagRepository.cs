using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.AI;
using Cortexa.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class RagRepository : GenericRepository<RAGQuery>, IRagRepository
    {
        public RagRepository(CortexaDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<RAGQuery> Items, int TotalCount)> GetPaginatedByPatientIdAsync(string patientId, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var query = _context.RAGQueries
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.QueryDateTime);

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}
