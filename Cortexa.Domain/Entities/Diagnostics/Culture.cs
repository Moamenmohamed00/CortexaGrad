using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class Culture : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid DoctorId { get; private set; }
        public Guid NurseId { get; private set; }

        public string CultureType { get; private set; }
        public string Result { get; private set; }
        public string Sensitivity { get; private set; }

        public DateTime SampleDate { get; private set; }
    }
}
