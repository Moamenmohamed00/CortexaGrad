using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Identity;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class LabResult : BaseEntity
    {
        public Guid LabOrderId { get; private set; }
        public LabOrder LabOrder { get; private set; }

        public Guid NurseId { get; private set; }
        public Nurse Nurse { get; private set; }
        public string Parameter { get; private set; }
        public float Value { get; private set; }
        public string Unit { get; private set; }
        public string ReferenceRange { get; private set; }

        public DateTime SampleDate { get; private set; }
    }
}
