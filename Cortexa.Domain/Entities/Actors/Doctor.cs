using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.StaffSchedule;
using Cortexa.Domain.Enums;
using System.Collections.Generic;

namespace Cortexa.Domain.Entities.Actors
{
    public class Doctor : AppUser
    {
         public string Specialty { get; set; } = string.Empty;

        public ShiftType Shift { get; set; }
        public DoctorRole Role { get; set; }
        public string Department { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }

        // Live availability status managed by the system (e.g., Available, InSurgery, Offline)
        public DoctorAvailabilityStatus AvailabilityStatus { get; set; }


        // Navigation Properties
        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
        public ICollection<VitalSigns> VerifiedVitalSigns { get; set; } = new List<VitalSigns>();
        public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
        public ICollection<Imaging> Imaging { get; set; } = new List<Imaging>();
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();
        public ICollection<Medications> Medications { get; set; } = new List<Medications>();
        public ICollection<PhysicalExamination> PhysicalExaminations { get; set; } = new List<PhysicalExamination>();
        public ICollection<CaseHistory> CaseHistories { get; set; } = new List<CaseHistory>();
        public ICollection<AlertOverrideLog> AlertOverrideLogs { get; set; } = new List<AlertOverrideLog>();
        public ICollection<RAGQuery> RAGQueries { get; set; } = new List<RAGQuery>();
        public ICollection<KnowledgeSource> KnowledgeSources { get; set; } = new List<KnowledgeSource>();

        public Doctor() { }

        // Core Domain Rule: Checks if the doctor is dynamically scheduled and present right now
        public bool IsCurrentlyOnDuty(DateTime currentDateTime)
        {
            // If the doctor manually set their status to offline/busy, they are not available
            if (AvailabilityStatus == DoctorAvailabilityStatus.Offline ||
                AvailabilityStatus == DoctorAvailabilityStatus.InSurgery)
            {
                return false;
            }

            // Check if there is an active schedule for this specific date and time
            return Schedules.Any(s =>
                s.ShiftStart <= currentDateTime &&
                s.ShiftEnd >= currentDateTime &&
                s.Status == ScheduleStatus.Present);
        }
    }

   
    
}
