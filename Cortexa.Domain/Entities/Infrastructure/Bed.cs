using Cortexa.Domain.Common;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Entities.Core; // For Admission reference

namespace Cortexa.Domain.Entities.Infrastructure
{
    public class Bed : BaseEntity
    {
        public string BedNumber { get; set; } = string.Empty;
        public BedStatus Status { get; set; }

        public string RoomId { get; set; } = string.Empty; // FK
        public Room Room { get; set; } = null!; // EF Core will set this

        public string? CurrentAdmissionId { get; set; }
        public Admission? CurrentAdmission { get; set; }
    }
}
