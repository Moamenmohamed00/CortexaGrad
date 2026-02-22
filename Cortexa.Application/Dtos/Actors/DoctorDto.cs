using System;
using System.Collections.Generic;
using Cortexa.Application.Dtos.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Actors
{
    public record DoctorDto(
        string Id,
        string Name,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        Gender Gender,
        AddressDto Address,
        string Specialty,
        ShiftType Shift,
        DoctorRole Role,
        string Department,
        int ExperienceYears
    );
}
