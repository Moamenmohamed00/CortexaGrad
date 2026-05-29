using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Beds
{
    public record BedDto
    (
        string BedId,
        string BedNumber ,
        BedStatus Status ,
        string? CurrentAdmissionId 
    );

    public record UpdateBedDto
    (
        string RoomId,
        string BedNumber,
        BedStatus Status
    );

}
