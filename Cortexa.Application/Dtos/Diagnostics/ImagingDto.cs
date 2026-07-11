using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Diagnostics
{
    public record ImagingDto(
        string Id,
        ImagingType Type,
        string? Findings,
        DateTime Date,
        string AdmissionId,
        string DoctorId,
    List<ImagingFileDto> Files
    );
    public record ImagingFileDto(
    string Id,
    string Url,
    string PublicId,
    string FileName,
    long? Size
);
}
