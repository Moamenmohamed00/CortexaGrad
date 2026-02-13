using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Identity;

namespace Cortexa.Domain.Entities.Admission
{
    public class Admission : BaseEntity
    {
        public Guid PatientId { get; private set; }

        public Patient Patient { get; private set; }
        public Guid DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }
        public Guid RoomId { get; private set; }
        public Room Room { get; private set; }

        public DateTime AdmissionDate { get; private set; } = DateTime.UtcNow;
        public DateTime? DischargeDate { get; private set; }

        public string InitialDiagnosis { get; private set; }
        public string Status { get; private set; } = "Active";

        
        

       

       

    }
}
