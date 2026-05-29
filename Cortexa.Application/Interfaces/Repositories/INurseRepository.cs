using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface INurseRepository : IGenericRepository<Nurse>
    {
        Task<IReadOnlyList<Nurse>> GetByDepartmentAsync(string department);
        Task<IReadOnlyList<Nurse>> GetAvailableNursesAsync();
        Task<Nurse?> GetByEmailAsync(string email);
    }
}
