using System;
using System.Collections.Generic;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Core
{
    public class Admission : BaseEntity
    {
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string InitialDiagnosis { get; set; } = string.Empty;
        public string DischargeSummary { get; set; } = string.Empty;
        public DischargeDisposition? DischargeDisposition { get; set; }
        public AdmissionStatus Status { get; set; }

        // Relationships
        public string PatientId { get; set; } = string.Empty;
        public Patient Patient { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty; // Admitting Doctor
        public Doctor Doctor { get; set; } = null!; // EF Core will set this

        public string? RoomId { get; set; }
        public Room? Room { get; set; }

        public string? BedId { get; set; }
        public Bed? Bed { get; set; }

        // Clinical Data Collections
        public ICollection<VitalSigns> VitalSigns { get; set; } = new List<VitalSigns>();
        public ICollection<FluidBalance> FluidBalances { get; set; } = new List<FluidBalance>();
        public ICollection<NursingNotes> NursingNotes { get; set; } = new List<NursingNotes>();
        public ICollection<CaseHistory> CaseHistories { get; set; } = new List<CaseHistory>();
        public ICollection<PhysicalExamination> PhysicalExaminations { get; set; } = new List<PhysicalExamination>();
        public ICollection<Medications> Medications { get; set; } = new List<Medications>();
        public ICollection<InterventionProcedure> InterventionProcedures { get; set; } = new List<InterventionProcedure>();

        // Diagnostics Collections
        public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
        public ICollection<Imaging> Imaging { get; set; } = new List<Imaging>();
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();

        // AI Collections
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();

        public Admission() { }
    }
}
