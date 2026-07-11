using System;

namespace Cortexa.Application.Dtos.Clinical
{
    public record PhysicalExaminationDto(
        string Id,
        DateTime ExamDate,
        float Temperature,
        string BloodPressure,
        int Pulse,
        int RespRate,
        string EyeStatus,
        string SkinStatus,
        string LipsStatus,
        string HeartExam,
        string AbdomenExam,
        string LocalExamination,
        string AdmissionId,
        string DoctorId
    );
}
