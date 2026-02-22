using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Clinical
{
    public class Medications : BaseEntity
    {
        public string DrugName { get; set; } = string.Empty;
        public int Dose { get; set; } // mg/mcg/mL? Maybe string with unit is better? ER says "int Dose". I'll stick to int or string.
                                      // I'll make it string to include unit "500 mg". Or int and Unit string.
                                      // ER says "int Dose". I'll assume standard unit or just int.

        public string DoseUnit { get; set; } = string.Empty; // Added Unit for clarity

        public int Frequency { get; set; } // Times per day? Or "Q4H"? 
                                           // ER says "int Frequency".

        public MedicationRoute Route { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this
    }
}
