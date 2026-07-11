using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Clinical
{
    public record InterventionProcedureDto(
        string Id,
        CareInterventionType Type,
        int Size,
        DateTime InsertionDate,
        DateTime? RemovalDate,
        string AdmissionId,
        string NurseId
    );
}
