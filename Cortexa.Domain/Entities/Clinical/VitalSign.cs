using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Identity;
using Cortexa.Domain.ValueObjects;
using Cortexa.Domain.Entities.Admission;

namespace Cortexa.Domain.Entities.Clinical
{
    public class VitalSign : BaseEntity
    {

        public Guid AdmissionId { get; private set; }
        public Cortexa.Domain.Entities.Admission.Admission Admission { get; private set; }

        public Guid NurseId { get; private set; }
        public Nurse Nurse { get; private set; }

        public Guid DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

        public DateTime RecordedAt { get; private set; }
        public float Temp { get; private set; }
        public int HeartRate { get; private set; }
        public int RespRate { get; private set; }
        public BloodPressure BloodPressure { get; private set; }
        public int PulseOxy { get; private set; }
        public int? CVP { get; private set; }
        public string InsulinGiven { get; private set; }

        public int GCS_Eye { get; private set; }
        public int GCS_Verbal { get; private set; }
        public int GCS_Motor { get; private set; }

        public int GCS_Total => GCS_Eye + GCS_Verbal + GCS_Motor;
    }
}
