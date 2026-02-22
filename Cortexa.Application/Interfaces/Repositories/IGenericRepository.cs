using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        //CancellationToken to stop query not be used to save recourcies
        Task<T> AddAsync(T entity, CancellationToken cancellationToken); // Added overload
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}