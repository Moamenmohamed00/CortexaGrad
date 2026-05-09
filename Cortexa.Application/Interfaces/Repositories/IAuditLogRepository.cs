using Cortexa.Application.Dtos.Core;
using Cortexa.Domain.Common;
using System.Linq.Expressions;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IAuditLogRepository 
    {
        Task<AuditLog?> GetByIdAsync(string id);
        Task<IReadOnlyList<AuditLog>> GetAllAsync();
        Task<AuditLog> AddAsync(AuditLog entity);
        //CancellationToken to stop query not be used to save recourcies
        Task<AuditLog> AddAsync(AuditLog entity, CancellationToken cancellationToken); // Added overload
        Task UpdateAsync(AuditLog entity);
        Task DeleteAsync(AuditLog entity);
        Task<IEnumerable<AuditLog>> FindAsync(
          Expression<Func<AuditLog, bool>> predicate,
          params Expression<Func<AuditLog, object>>[] includes);
        Task<PagedResult<AuditLog>> GetPagedAsync(
int pageNumber,
int pageSize,
Expression<Func<AuditLog, bool>>? filter = null,
Func<IQueryable<AuditLog>, IOrderedQueryable<AuditLog>>? orderBy = null);
    }
}
