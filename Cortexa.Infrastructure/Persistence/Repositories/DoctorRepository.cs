using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
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
    }
}
