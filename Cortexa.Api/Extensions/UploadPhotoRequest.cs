using Cortexa.Domain.Enums;

namespace Cortexa.Api.Extensions
{
    public class UploadImagingRequest
    {
        public List<IFormFile> Files { get; set; } = new();
        public string AdmissionId { get; set; } = string.Empty;
        public ImagingType Type { get; set; }
        public string? Findings { get; set; }
        public DateTime Date { get; set; }
        public string DoctorId { get; set; } = string.Empty;
    }
}
