using Cortexa.Application.Dtos.Beds;
using Cortexa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Rooms
{
    public record RoomDto
(
   string RoomId,
   string RoomNumber,
   RoomType Type,
   int Floor,
   IReadOnlyList<BedDto> Beds
);
}
