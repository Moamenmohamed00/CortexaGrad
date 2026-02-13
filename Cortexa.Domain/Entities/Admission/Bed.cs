using Cortexa.Domain.Common;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Admission
{
    public class Bed : BaseEntity
    {
        public Guid RoomId { get; private set; }
        public int BedNumber { get; private set; }
        public BedStatus Status { get; private set; } = BedStatus.Empty;
        public Guid? CurrentAdmissionId { get; private set; }

        public Admission Admission { get; private set; }

    }
}
