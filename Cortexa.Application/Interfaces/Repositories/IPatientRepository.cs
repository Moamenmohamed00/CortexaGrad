using Cortexa.Domain.Entities.Actors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByNationalIdAsync(string nationalId,CancellationToken cancellationToken);
        Task<IReadOnlyList<Patient>> GetActivePatientsAsync();

        Task<string?> GetPatientNameByPatientIdAsync(string patientId);
        // add nationid properaty in configration file to all classes
        //we need indexer to search by name
    }
}
