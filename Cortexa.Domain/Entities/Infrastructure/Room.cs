using System.Collections.Generic;
using Cortexa.Domain.Common;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Infrastructure
{
    public class Room : BaseEntity
    {
        public string RoomNumber { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public int Floor { get; set; }

        public ICollection<Bed> Beds { get; set; } = new List<Bed>();
    }
}
