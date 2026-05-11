using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        protected readonly CortexaDbContext _context;

        public AuditLogRepository(CortexaDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog?> GetByIdAsync(string id)
        {
            return await _context.Set<AuditLog>().FindAsync(id);
        }

        public async Task<IReadOnlyList<AuditLog>> GetAllAsync()
        {
            return await _context.Set<AuditLog>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AuditLog> AddAsync(AuditLog entity)
        {
            await _context.Set<AuditLog>().AddAsync(entity);
            return entity;
        }

        public async Task<AuditLog> AddAsync(AuditLog entity, CancellationToken cancellationToken)
        {
            await _context.Set<AuditLog>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public Task UpdateAsync(AuditLog entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(AuditLog entity)
        {
            _context.Set<AuditLog>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<AuditLog>> FindAsync(
    Expression<Func<AuditLog, bool>> predicate,
    params Expression<Func<AuditLog, object>>[] includes)
        {
            IQueryable<AuditLog> query = _context.Set<AuditLog>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<PagedResult<AuditLog>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    Expression<Func<AuditLog, bool>>? filter = null,
    Func<IQueryable<AuditLog>, IOrderedQueryable<AuditLog>>? orderBy = null)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be greater than 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0.");

            IQueryable<AuditLog> query = _context
                .Set<AuditLog>()
                .AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            int totalCount = await query.CountAsync();

            if (orderBy is not null)
                query = orderBy(query);

            List<AuditLog> items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<AuditLog>
            (
               pageNumber,
               pageSize,
               totalCount,
               items
            );
        }

    }
    
}
