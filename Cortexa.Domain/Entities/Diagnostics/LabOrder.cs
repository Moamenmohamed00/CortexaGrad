using System;
using System.Collections.Generic;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class LabOrder : BaseEntity
    {
        public string TestName { get; set; } = string.Empty; // e.g., "CBC", "BMP"
        public DateTime OrderDate { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this

        public ICollection<LabResult> Results { get; set; } = new List<LabResult>();
    }
}
