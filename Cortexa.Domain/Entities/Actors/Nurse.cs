using System.Collections.Generic;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Actors
{
    public class Nurse : AppUser
    {
        public ShiftType Shift { get; set; }
        public NurseRole Role { get; set; }
        public string Department { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<VitalSigns> RecordedVitalSigns { get; set; } = new List<VitalSigns>();
        public ICollection<FluidBalance> FluidBalances { get; set; } = new List<FluidBalance>();
        public ICollection<NursingNotes> NursingNotes { get; set; } = new List<NursingNotes>();
        public ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();
        public ICollection<InterventionProcedure> InterventionProcedures { get; set; } = new List<InterventionProcedure>();

        public Nurse() { }
    }
}
