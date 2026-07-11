using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.StaffSchedule;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface INurseRepository : IGenericRepository<Nurse>
    {
        Task<IReadOnlyList<Nurse>> GetByDepartmentAsync(string department);
        Task<IReadOnlyList<Nurse>> GetAvailableNursesAsync();
        Task<Nurse?> GetByEmailAsync(string email);
        Task AddRangeSchedulesAsync(IEnumerable<NurseSchedule> schedules);
        Task<IReadOnlyList<NurseSchedule>> GetSchedulesByNurseIdAsync(string nurseId);
        Task<int> CountActiveNursesTodayAsync(DateTime date, CancellationToken cancellationToken);
    }
}
