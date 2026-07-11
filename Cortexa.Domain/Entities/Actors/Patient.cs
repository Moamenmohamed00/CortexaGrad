using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using System.Collections.Generic;

namespace Cortexa.Domain.Entities.Actors
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;
        public string NationalId { get; set; } = string.Empty;

        public string FileNumber { get; set; } = string.Empty;
        public string? DiagnosisSummary { get; set; }
        public BloodType BloodType { get; set; }

        public int Age => DateTime.UtcNow.Year - DateOfBirth.Year -
                         (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

        public string Sex => Gender.ToString();

        public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
        public ICollection<RAGQuery> RAGQueries { get; set; } = new List<RAGQuery>();

        public Patient() { }
    }
}
