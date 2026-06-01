namespace Cortexa.Domain.Enums
{
    public enum ShiftType
    {
        Morning,
        Evening,
        Night
    }
    public enum ScheduleStatus
    {
        Scheduled,
        Present,
        OnLeave,
        Absent
    }

    public enum DoctorAvailabilityStatus
    {
        Available,
        InSurgery,
        OnBreak,
        Offline
    }
    public enum NurseAvailabilityStatus
    {
        Available,
        AssignedToPatient, // ICU nurses often get locked to specific beds/patients during shifts
        OnBreak,
        Offline
    }
}
