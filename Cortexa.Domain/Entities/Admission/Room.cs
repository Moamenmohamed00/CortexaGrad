using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Admission
{
    public class Room : BaseEntity
    {
        public int RoomNumber { get; private set; }
        public string Type { get; private set; }
        public int Floor { get; private set; }

        public ICollection<Bed> Beds { get; private set; } = new List<Bed>();

    }
}
