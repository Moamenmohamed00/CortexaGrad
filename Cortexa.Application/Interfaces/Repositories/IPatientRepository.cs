using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByNationalIdAsync(string nationalId);
        Task<IReadOnlyList<Patient>> GetActivePatientsAsync();
        // add nationid properaty in configration file to all classes
        //we need indexer to search by name
    }
}
