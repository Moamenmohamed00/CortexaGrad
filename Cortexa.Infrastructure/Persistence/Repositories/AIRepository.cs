using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.AI;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class AIRepository : GenericRepository<Alert>, IAIRepository
    {
        public AIRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Alert>> GetAlertsByPatientIdAsync(string patientId)
        {
            return await _context.Alerts
                .Where(a => _context.Admissions.Any(adm => adm.PatientId == patientId && adm.Id == a.AdmissionId))
                .OrderByDescending(a => a.GeneratedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Alert>> GetAlertsByAdmissionIdAsync(string admissionId)
        {
            return await _context.Alerts
                .Where(a => a.AdmissionId == admissionId)
                .OrderByDescending(a => a.GeneratedAt)
                .ToListAsync();
        }
    }
}
