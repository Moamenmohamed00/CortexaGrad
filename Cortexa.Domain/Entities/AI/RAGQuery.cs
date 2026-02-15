using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.AI
{
    public class RAGQuery : BaseEntity
    {
        public string QueryText { get; set; } = string.Empty;
        public float ScoreTrust { get; set; }
        public RelevanceLevel RelevanceLevel { get; set; }
        public DateTime QueryDateTime { get; set; }

        // Response?
        public string GeneratedResponse { get; set; } = string.Empty;

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this

        public string? PatientId { get; set; } // Optional context
        public Patient? Patient { get; set; }
    }
}
