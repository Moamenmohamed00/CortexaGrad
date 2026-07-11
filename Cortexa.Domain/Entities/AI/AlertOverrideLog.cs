using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Domain.Entities.AI
{
    public class AlertOverrideLog : BaseEntity
    {
        /*change Status Severity*/
        public string Reason { get; set; } = string.Empty;
        public DateTime OverrideTime { get; set; }

        public string AlertId { get; set; } = string.Empty;
        public Alert Alert { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this

        // Optional link to procedure if override was due to a procedure
        public string? ProcedureId { get; set; }
        public InterventionProcedure? Procedure { get; set; }
    }
}
