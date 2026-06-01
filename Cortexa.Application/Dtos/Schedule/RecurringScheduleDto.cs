namespace Cortexa.Application.Dtos.Schedule
{
    public record RecurringScheduleDto(
    string Id,
    string DoctorId,
    DateTime StartDate,
    DateTime EndDate,
    List<DayOfWeek> DaysOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime
);
}
