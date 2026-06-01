using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.StaffSchedule;
namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IReadOnlyList<Doctor>> GetBySpecializationAsync(string specialization);
        Task<IReadOnlyList<Doctor>> GetAvailableDoctorsAsync();

        Task<Doctor?> GetByEmailAsync(string email);

        Task AddRangeSchedulesAsync(IEnumerable<DoctorSchedule> schedules);

        Task<IReadOnlyList<DoctorSchedule>> GetSchedulesByDoctorIdAsync(string doctorId);

        Task<int> CountActiveDoctorsTodayAsync(DateTime date, CancellationToken cancellationToken);
    }
}
