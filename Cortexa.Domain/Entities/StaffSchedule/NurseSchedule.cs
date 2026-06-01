using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.StaffSchedule
{
    public class NurseSchedule : BaseEntity
    {
        public string NurseId { get; set; }  =string.Empty;
        public Nurse Nurse { get; set; } = null!;

        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }

        public ScheduleStatus Status { get; set; }
    }
}
