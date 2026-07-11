using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(CortexaDbContext context) : base(context) { }

        public async Task<IEnumerable<Room>> GetRoomsWithBedsAvailableAsync()
        {
            return await _context.Rooms
                .Include(r => r.Beds)
                .ToListAsync();

        }
    }
}
