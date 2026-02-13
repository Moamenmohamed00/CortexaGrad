using Cortexa.Domain.Common;
using Cortexa.Domain.ValueObjects;

namespace Cortexa.Domain.Entities.Admission
{
    public class Patient : BaseEntity
    {
        public string FileNumber { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Sex { get; private set; }
        public string DiagnosisSummary { get; private set; }
        public string BloodType { get; private set; }
        public string Phone { get; private set; }
        public Address Address { get; private set; }


        public ICollection<Admission> Admissions { get; private set; } = new List<Admission>();

    }
}
