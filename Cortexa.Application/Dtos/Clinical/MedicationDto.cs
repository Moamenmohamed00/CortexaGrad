using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Clinical
{
    public record MedicationDto(
        string Id,
        string DrugName,
        int Dose,
        string DoseUnit,
        int Frequency,
        MedicationRoute Route,
        DateTime StartDate,
        DateTime? EndDate,
        string AdmissionId,
        string DoctorId
    );
}
