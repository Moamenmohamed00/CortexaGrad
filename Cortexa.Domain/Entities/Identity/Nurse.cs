using Cortexa.Domain.Common;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Identity
{
    public class Nurse : BaseEntity
    {
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Department { get; private set; }

        public ShiftType Shift { get; private set; }
        public StaffRole Role { get; private set; }

    }
}
