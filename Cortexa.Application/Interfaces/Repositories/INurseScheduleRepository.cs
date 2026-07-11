using Cortexa.Domain.Entities.StaffSchedule;
namespace Cortexa.Application.Interfaces.Repositories
{
    public interface INurseScheduleRepository : IGenericRepository<NurseSchedule>
    {
        Task<IReadOnlyList<NurseSchedule>> GetSchedulesByNurseIdAsync(string nurseId);
    }
}
