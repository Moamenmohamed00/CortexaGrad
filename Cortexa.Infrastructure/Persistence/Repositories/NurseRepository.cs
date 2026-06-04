using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.StaffSchedule;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class NurseRepository : GenericRepository<Nurse>, INurseRepository
    {
        public NurseRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Nurse>> GetAvailableNursesAsync()
        {
            return await _context.Nurses.ToListAsync();
        }

        public async Task<IReadOnlyList<Nurse>> GetByDepartmentAsync(string department)
        {
            return await _context.Nurses.Where(n => n.Department == department).ToListAsync();
        }

        public async Task<Nurse?> GetByEmailAsync(string email)
        {
            return await _context.Nurses.FirstOrDefaultAsync(n => n.Email == email);
        }
        public async Task AddRangeSchedulesAsync(IEnumerable<NurseSchedule> schedules)
        {
            await _context.NurseSchedules.AddRangeAsync(schedules);
        }

        public async Task<IReadOnlyList<NurseSchedule>> GetSchedulesByNurseIdAsync(string nurseId)
        {
            return await _context.NurseSchedules
                .Where(ns => ns.NurseId == nurseId)
                .ToListAsync();
        }

        public Task<int> CountActiveNursesTodayAsync(DateTime date, CancellationToken cancellationToken)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);
            return _context.NurseSchedules
                .Where(ns =>
                    ns.ShiftStart < endOfDay &&
                    ns.ShiftEnd > startOfDay &&
                    (ns.Status == ScheduleStatus.Scheduled || ns.Status == ScheduleStatus.Present))
                .Select(ns => ns.NurseId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

    }
}
