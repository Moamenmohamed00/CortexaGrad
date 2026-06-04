namespace Cortexa.Application.Dtos.Schedule
{
    public record StaffScheduleDto(
    string Id,
    string StaffId,
    DateTime StartDate,
    DateTime EndDate,
    List<DayOfWeek> DaysOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime
);
}
