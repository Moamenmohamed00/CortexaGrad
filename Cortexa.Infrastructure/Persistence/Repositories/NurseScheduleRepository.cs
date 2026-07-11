using Cortexa.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Cortexa.Domain.Entities.StaffSchedule;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class NurseScheduleRepository : GenericRepository<NurseSchedule>, INurseScheduleRepository
    {
        public NurseScheduleRepository(CortexaDbContext context) : base(context) { }
        public async Task<IReadOnlyList<NurseSchedule>> GetSchedulesByNurseIdAsync(string nurseId)
        {
            return await _context.NurseSchedules
                .Where(s => s.NurseId == nurseId)
                .ToListAsync();
        }
    }
}
