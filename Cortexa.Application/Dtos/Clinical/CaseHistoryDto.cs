using System;

namespace Cortexa.Application.Dtos.Clinical
{
    public record CaseHistoryDto(
        string Id,
        string Complaint,
        string PresentIllness,
        string? ChronicDisease,
        string? GeneticDisease,
        string? MaritalHistory,
        string? SpecialHabits,
        string? ClinicalNotes,
        string AdmissionId,
        string DoctorId
    );
}
