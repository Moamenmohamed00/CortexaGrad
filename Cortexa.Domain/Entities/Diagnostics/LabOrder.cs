using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Identity;
namespace Cortexa.Domain.Entities.Diagnostics
{
    public class LabOrder : BaseEntity
    {

        public Guid AdmissionId { get; private set; }
        public Cortexa.Domain.Entities.Admission.Admission Admission { get; private set; }

        public Guid DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

        public ICollection<LabResult> LabResults { get; private set; } = new List<LabResult>();

        public string TestName { get; private set; }
        public DateTime OrderDate { get; private set; }

        
    }
}
