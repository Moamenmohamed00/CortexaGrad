using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(CortexaDbContext context) : base(context) { }

        public async Task<Patient?> GetByNationalIdAsync(string nationalId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.NationalId == nationalId);
        }

        public async Task<IReadOnlyList<Patient>> GetActivePatientsAsync()
        {
            return await _context.Patients
                .Include(p => p.Admissions)
                .Where(p => p.Admissions.Any(a => a.Status == AdmissionStatus.Active))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
