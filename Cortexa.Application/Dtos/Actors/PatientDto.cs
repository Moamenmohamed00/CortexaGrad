using System;
using System.Collections.Generic;
using Cortexa.Application.Dtos.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Actors
{
    public record PatientDto(
        string Id,
        string Name,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        Gender Gender,
        AddressDto Address,
        string FileNumber,
        string? DiagnosisSummary,
        BloodType BloodType,
        int Age,
        string Sex
    );
}
