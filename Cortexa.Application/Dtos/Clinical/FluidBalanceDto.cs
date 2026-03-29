using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Clinical
{
    public record FluidBalanceDto
    {
        public string Id { get; init; } = string.Empty;
        public DateTime RecordedAt { get; init; }
        public FluidBalanceCategory Category { get; init; }
        public FluidType Type { get; init; }
        public int AmountMl { get; init; }
        public string AdmissionId { get; init; } = string.Empty;
        public string NurseId { get; init; } = string.Empty;
    }
}
