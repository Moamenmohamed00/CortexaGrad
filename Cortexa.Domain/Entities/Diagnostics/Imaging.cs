using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class Imaging : BaseEntity
    {
        public ImagingType Type { get; set; }
        public string? Findings { get; set; }
        public DateTime Date { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this

        public ICollection<ImagingFile> Files { get; set; } = new List<ImagingFile>();
    }
    public class ImagingFile : BaseEntity
    {
        public string Url { get; set; } = null!;

        public string PublicId { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public long? Size { get; set; }

        public string ImagingId { get; set; } = null!;
        public Imaging Imaging { get; set; } = null!;
    }
}
