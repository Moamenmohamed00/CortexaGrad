using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Clinical
{
    public class Medication : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid DoctorId { get; private set; }

        public string DrugName { get; private set; }
        public int Dose { get; private set; }
        public int Frequency { get; private set; }
        public string Route { get; private set; }

        public DateOnly StartDate { get; private set; }
        public DateOnly? EndDate { get; private set; }

    }
}
