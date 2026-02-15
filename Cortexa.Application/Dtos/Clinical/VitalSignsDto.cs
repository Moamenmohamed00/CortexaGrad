using System;

namespace Cortexa.Application.Dtos.Clinical
{
    public record VitalSignsDto(
        string Id,
        DateTime RecordedAt,
        float Temperature,
        int HeartRate,
        int RespRate,
        int BpSystolic,
        int BpDiastolic,
        int PulseOxy,
        int Cvp,
        string InsulinGiven,
        int GcsEye,
        int GcsVerbal,
        int GcsMotor,
        int GcsTotal,
        string AdmissionId,
        string NurseId,
        string? DoctorId
    );
}
