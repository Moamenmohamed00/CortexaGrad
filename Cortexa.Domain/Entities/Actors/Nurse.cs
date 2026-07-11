using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.StaffSchedule;
using Cortexa.Domain.Enums;
using System.Collections.Generic;

namespace Cortexa.Domain.Entities.Actors
{
    public class Nurse : AppUser
    {
        public ShiftType Shift { get; set; }
        public NurseRole Role { get; set; }
        public string Department { get; set; } = string.Empty;

        public NurseAvailabilityStatus AvailabilityStatus { get; set; }

        // Navigation Properties
        public ICollection<NurseSchedule> Schedules { get; set; } = new List<NurseSchedule>();
        public ICollection<VitalSigns> RecordedVitalSigns { get; set; } = new List<VitalSigns>();
        public ICollection<FluidBalance> FluidBalances { get; set; } = new List<FluidBalance>();
        public ICollection<NursingNotes> NursingNotes { get; set; } = new List<NursingNotes>();
        public ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();
        public ICollection<InterventionProcedure> InterventionProcedures { get; set; } = new List<InterventionProcedure>();

        public Nurse() { }

        public bool IsCurrentlyOnDuty(DateTime currentDateTime)
        {
            if (AvailabilityStatus == NurseAvailabilityStatus.Offline ||
                AvailabilityStatus == NurseAvailabilityStatus.OnBreak)
            {
                return false;
            }

            return Schedules.Any(s =>
                s.ShiftStart <= currentDateTime &&
                s.ShiftEnd >= currentDateTime &&
                s.Status == ScheduleStatus.Present);
        }
    }
}
