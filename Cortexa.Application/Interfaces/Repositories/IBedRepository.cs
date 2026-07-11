using Cortexa.Domain.Entities.Infrastructure;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IBedRepository : IGenericRepository<Bed>
    {
        Task<IReadOnlyList<Bed>> GetOccupiedBedsAsync(CancellationToken cancellationToken = default);
    }


}
