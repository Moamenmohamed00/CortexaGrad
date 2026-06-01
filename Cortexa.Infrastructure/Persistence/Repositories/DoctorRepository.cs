using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.StaffSchedule;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Doctor>> GetBySpecializationAsync(string specialization)
        {
            return await _context.Doctors
                .Where(d => d.Specialty == specialization)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Doctor>> GetAvailableDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public Task<Doctor?> GetByEmailAsync(string email)
        {
            return _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task AddRangeSchedulesAsync(IEnumerable<DoctorSchedule> schedules)
        {
            await _context.DoctorSchedules.AddRangeAsync(schedules);
        }

        public async Task<IReadOnlyList<DoctorSchedule>> GetSchedulesByDoctorIdAsync(string doctorId) 
        {
            return await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId)
                .ToListAsync();
        }

        public Task<int> CountActiveDoctorsTodayAsync(DateTime date,CancellationToken cancellationToken)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return _context.DoctorSchedules
                .Where(ds =>
                    ds.ShiftStart < endOfDay &&
                    ds.ShiftEnd > startOfDay &&
                    (ds.Status == ScheduleStatus.Scheduled || ds.Status == ScheduleStatus.Present))
                .Select(ds => ds.DoctorId)
                .Distinct()
                .CountAsync(cancellationToken);
        }
    }
}
