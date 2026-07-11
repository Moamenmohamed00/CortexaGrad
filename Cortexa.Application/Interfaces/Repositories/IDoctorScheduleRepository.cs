using Cortexa.Domain.Entities.StaffSchedule;
namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule>
    {
        Task<IReadOnlyList<DoctorSchedule>> GetSchedulesByDoctorIdAsync(string doctorId);
    }
}
