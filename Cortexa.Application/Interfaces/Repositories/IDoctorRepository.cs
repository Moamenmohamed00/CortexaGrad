using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IReadOnlyList<Doctor>> GetBySpecializationAsync(string specialization);
        Task<IReadOnlyList<Doctor>> GetAvailableDoctorsAsync();
    }
}
