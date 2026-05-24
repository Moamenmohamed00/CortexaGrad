using Cortexa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Diagnostics
{
    public class UploadImagingDto
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string AdmissionId { get; set; } = string.Empty;
        public ImagingType Type { get; set; }
        public string? Findings { get; set; }
        public DateTime Date { get; set; }
        public string DoctorId { get; set; } = string.Empty;


    }
}
