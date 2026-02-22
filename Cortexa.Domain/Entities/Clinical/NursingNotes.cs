using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Domain.Entities.Clinical
{
    public class NursingNotes : BaseEntity
    {
        public string NoteText { get; set; } = string.Empty;
        public DateTime NoteDateTime { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string NurseId { get; set; } = string.Empty;
        public Nurse Nurse { get; set; } = null!; // EF Core will set this
    }
}
