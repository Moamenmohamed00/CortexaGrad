using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.Persistence.Seeding
{
    /// <summary>
    /// Seeds master/reference data: Rooms and Beds.
    /// This data is rarely changed and forms the hospital's physical infrastructure.
    /// </summary>
    public class MasterDataSeeder
    {
        private readonly CortexaDbContext _context;
        private readonly ILogger<MasterDataSeeder> _logger;

        public MasterDataSeeder(CortexaDbContext context, ILogger<MasterDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (await _context.Rooms.AnyAsync())
            {
                _logger.LogInformation("Master data already seeded. Skipping.");
                return;
            }

            _logger.LogInformation("Seeding master data (Rooms & Beds)...");

            var rooms = CreateRooms();
            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {RoomCount} rooms with beds.", rooms.Count);
        }

        private List<Room> CreateRooms()
        {
            var rooms = new List<Room>();

            // ── Floor 1: Emergency Department ──────────────────────────
            rooms.Add(CreateRoom("ER-101", RoomType.ER, 1, new[] { "ER-101-A", "ER-101-B", "ER-101-C" }));
            rooms.Add(CreateRoom("ER-102", RoomType.ER, 1, new[] { "ER-102-A", "ER-102-B" }));

            // ── Floor 2: General Ward ──────────────────────────────────
            rooms.Add(CreateRoom("W-201", RoomType.Ward, 2, new[] { "W-201-A", "W-201-B", "W-201-C", "W-201-D" }));
            rooms.Add(CreateRoom("W-202", RoomType.Ward, 2, new[] { "W-202-A", "W-202-B", "W-202-C", "W-202-D" }));
            rooms.Add(CreateRoom("W-203", RoomType.Ward, 2, new[] { "W-203-A", "W-203-B" }));

            // ── Floor 2: Private Rooms ─────────────────────────────────
            rooms.Add(CreateRoom("P-204", RoomType.Private, 2, new[] { "P-204-A" }));
            rooms.Add(CreateRoom("P-205", RoomType.Private, 2, new[] { "P-205-A" }));

            // ── Floor 3: ICU ───────────────────────────────────────────
            rooms.Add(CreateRoom("ICU-301", RoomType.ICU, 3, new[] { "ICU-301-A", "ICU-301-B" }));
            rooms.Add(CreateRoom("ICU-302", RoomType.ICU, 3, new[] { "ICU-302-A", "ICU-302-B" }));

            // ── Floor 3: Operating Room ────────────────────────────────
            rooms.Add(CreateRoom("OR-303", RoomType.OR, 3, new[] { "OR-303-A" }));

            return rooms;
        }

        private static Room CreateRoom(string roomNumber, RoomType type, int floor, string[] bedNumbers)
        {
            var room = new Room
            {
                RoomNumber = roomNumber,
                Type = type,
                Floor = floor
            };

            foreach (var bedNumber in bedNumbers)
            {
                room.Beds.Add(new Bed
                {
                    BedNumber = bedNumber,
                    Status = BedStatus.Available,
                    RoomId = room.Id
                });
            }

            return room;
        }
    }
}
