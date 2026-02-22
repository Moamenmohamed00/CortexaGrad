using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Clinical
{
    public class InterventionProcedure : BaseEntity
    {
        public CareInterventionType Type { get; set; } // CVL, Cannula, Foley
        public int Size { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? RemovalDate { get; set; }

        // Relationships
        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string NurseId { get; set; } = string.Empty;
        public Nurse Nurse { get; set; } = null!; // EF Core will set this
    }
}
