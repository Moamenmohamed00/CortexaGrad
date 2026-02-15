using System.Collections.Generic;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Actors
{
    public class Patient : AppUser
    {
        public string FileNumber { get; set; } = string.Empty;
        public string? DiagnosisSummary { get; set; }
        public BloodType BloodType { get; set; }

        // ER Diagram: Age (computed from DateOfBirth in AppUser)
        public int Age => GetAge();

        // ER Diagram: Sex (string) - derived from Gender enum or can be set separately
        public string Sex => Gender.ToString();

        // Relationships
        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
        public ICollection<RAGQuery> RAGQueries { get; set; } = new List<RAGQuery>();

        public Patient()
        {
        }
    }
}
