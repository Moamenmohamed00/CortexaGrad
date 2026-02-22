using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class AdmissionRepository : GenericRepository<Admission>, IAdmissionRepository
    {
        public AdmissionRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Admission>> GetActiveAdmissionsAsync()
        {
            return await _context.Admissions
                .Where(a => a.Status == AdmissionStatus.Active)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Bed)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Admission>> GetAdmissionsByPatientIdAsync(string patientId)
        {
            return await _context.Admissions
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AdmissionDate)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
