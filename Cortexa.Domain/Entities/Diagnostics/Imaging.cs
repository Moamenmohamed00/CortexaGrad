using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class Imaging : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid DoctorId { get; private set; }

        public string Findings { get; private set; }
        public string Type { get; private set; }
        public DateTime Date { get; private set; }

    }
}
