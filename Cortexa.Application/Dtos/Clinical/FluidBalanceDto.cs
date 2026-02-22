using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Clinical
{
    public record FluidBalanceDto(
        string Id,
        DateTime RecordedAt,
        FluidBalanceCategory Category,
        FluidType Type,
        int AmountMl,
        string AdmissionId,
        string NurseId
    );
}
