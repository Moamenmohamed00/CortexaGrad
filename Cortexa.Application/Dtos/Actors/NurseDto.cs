using System;
using System.Collections.Generic;
using Cortexa.Application.Dtos.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Actors
{
    public record NurseDto(
        string Id,
        string Name,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        Gender Gender,
        AddressDto Address,
        ShiftType Shift,
        NurseRole Role,
        string Department
    );
}
