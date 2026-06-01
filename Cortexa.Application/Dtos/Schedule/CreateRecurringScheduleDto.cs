using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Schedule
{
    public record CreateRecurringScheduleDto(
        DateTime StartDate,
        DateTime EndDate,
        List<DayOfWeek> DaysOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime
    );
}
