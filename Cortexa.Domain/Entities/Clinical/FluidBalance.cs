using Cortexa.Domain.Common;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Clinical
{
    public class FluidBalance : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid NurseId { get; private set; }

        public DateTime RecordedAt { get; private set; } = DateTime.UtcNow;

        public string Category { get; private set; }
        public FluidType Type { get; private set; }
        public int AmountML { get; private set; }
    }
}
