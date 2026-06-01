using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Entities.StaffSchedule;
using Cortexa.Domain.Enums;

namespace Cortexa.Infrastructure.Services
{
    public class ScheduleApplicationService : IScheduleApplicationService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleApplicationService(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork)
        {
            _doctorRepository = doctorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<bool>> GenerateRecurringSchedulesAsync(string doctorId, CreateRecurringScheduleDto command)
        {
            // 1. Validate that the doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return ResultDto<bool>.Failure($"Doctor with ID {doctorId} not found.");
            }

            var generatedSchedules = new List<DoctorSchedule>();

            // 2. Loop through every single day in the date range
            for (var currentDate = command.StartDate.Date; currentDate <= command.EndDate.Date; currentDate = currentDate.AddDays(1))
            {
                // 3. Check if the current day of the week matches the manager's selection
                if (command.DaysOfWeek.Contains(currentDate.DayOfWeek))
                {
                    var shiftStart = currentDate.Add(command.StartTime);
                    var shiftEnd = currentDate.Add(command.EndTime);

                    // Critical Edge-Case: If the shift is a night shift (e.g., 22:00 to 06:00), 
                    // the end time belongs to the next calendar day.
                    if (shiftEnd <= shiftStart)
                    {
                        shiftEnd = shiftEnd.AddDays(1);
                    }

                    var newSchedule = new DoctorSchedule
                    {
                        DoctorId = doctorId,
                        ShiftStart = shiftStart,
                        ShiftEnd = shiftEnd,
                        Status = ScheduleStatus.Scheduled,
                        IsOnCall = false
                    };

                    generatedSchedules.Add(newSchedule);
                }
            }

            // 4. Bulk insert into the database via Repository / DbContext
            if (generatedSchedules.Any())
            {
                await _doctorRepository.AddRangeSchedulesAsync(generatedSchedules);
                await _unitOfWork.SaveChangesAsync();
            }

            return ResultDto<bool>.SuccessResult(true, "Recurring schedules generated successfully.");
        }

        public async Task<ResultDto<IEnumerable<RecurringScheduleDto>>> GetSchedulesByDoctorIdAsync(string doctorId)
        {
            // 1. Validate that the doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return ResultDto<IEnumerable<RecurringScheduleDto>>.Failure($"Doctor with ID {doctorId} not found.");
            }



            // 2. Retrieve the recurring schedules for the doctor
            var recurringSchedules = await _doctorRepository.GetSchedulesByDoctorIdAsync(doctorId);

            // 3. Map the recurring schedules to DTOs
            var recurringScheduleDtos = recurringSchedules.Select(schedule => new RecurringScheduleDto
            (
                Id: schedule.Id,
                DoctorId: schedule.DoctorId,
                StartDate: schedule.ShiftStart.Date,
                EndDate: schedule.ShiftEnd.Date,
                DaysOfWeek: GetDaysOfWeek(schedule.ShiftStart, schedule.ShiftEnd),
                StartTime: schedule.ShiftStart.TimeOfDay,
                EndTime: schedule.ShiftEnd.TimeOfDay
            ));

            return ResultDto<IEnumerable<RecurringScheduleDto>>.SuccessResult(recurringScheduleDtos, "Recurring schedules retrieved successfully.");
        }

        private List<DayOfWeek> GetDaysOfWeek(DateTime shiftStart, DateTime shiftEnd)
        {
            var daysOfWeek = new List<DayOfWeek>();
            for (var date = shiftStart.Date; date <= shiftEnd.Date; date = date.AddDays(1))
            {
                if (!daysOfWeek.Contains(date.DayOfWeek))
                {
                    daysOfWeek.Add(date.DayOfWeek);
                }
            }
            return daysOfWeek;
        }
    }
}
