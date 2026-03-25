using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
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

        public Task<Nurse?> GetByEmailAsync(string email)
        {
            return _context.Nurses.FirstOrDefaultAsync(n => n.Email == email);
        }
    }
}
