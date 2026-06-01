using Cortexa.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Cortexa.Domain.Entities.StaffSchedule;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class DoctorScheduleRepository : GenericRepository<DoctorSchedule>, IDoctorScheduleRepository
    {
        public DoctorScheduleRepository(CortexaDbContext context) : base(context) { }
        public async Task<IReadOnlyList<DoctorSchedule>> GetSchedulesByDoctorIdAsync(string doctorId)
        {
            return await _context.DoctorSchedules
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();
        }
    }
}
