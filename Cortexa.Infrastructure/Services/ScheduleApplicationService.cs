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
        private readonly INurseRepository _nurseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleApplicationService(IDoctorRepository doctorRepository, INurseRepository nurseRepository, IUnitOfWork unitOfWork)
        {
            _doctorRepository = doctorRepository;
            _nurseRepository = nurseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<bool>> GenerateDoctorSchedulesAsync(string doctorId, CreateStaffScheduleDto command)
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

            return ResultDto<bool>.SuccessResult(true, "Doctor schedules generated successfully.");
        }

        public async Task<ResultDto<bool>> GenerateNurseSchedulesAsync(string nurseId, CreateStaffScheduleDto command)
        {
            // 1. Validate that the nurse exists
            var nurse = await _nurseRepository.GetByIdAsync(nurseId);
            if (nurse == null)
            {
                return ResultDto<bool>.Failure($"Nurse with ID {nurseId} not found.");
            }

            var generatedSchedules = new List<NurseSchedule>();

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

                    var newSchedule = new NurseSchedule
                    {
                        NurseId = nurseId,
                        ShiftStart = shiftStart,
                        ShiftEnd = shiftEnd,
                        Status = ScheduleStatus.Scheduled,
                    };

                    generatedSchedules.Add(newSchedule);
                }
            }

            // 4. Bulk insert into the database via Repository / DbContext
            if (generatedSchedules.Any())
            {
                await _nurseRepository.AddRangeSchedulesAsync(generatedSchedules);
                await _unitOfWork.SaveChangesAsync();
            }

            return ResultDto<bool>.SuccessResult(true, "Nurse schedules generated successfully.");
        }

        public Task<ResultDto<bool>> GenerateStaffSchedulesAsync(string staffId, CreateStaffScheduleDto command)
        {
            if (!string.IsNullOrEmpty(staffId)) 
            {
                if(staffId.StartsWith("DOC"))
                {
                    return GenerateDoctorSchedulesAsync(staffId, command);
                }
                else if (staffId.StartsWith("NUR"))
                {
                    return GenerateNurseSchedulesAsync(staffId, command);
                }
                else
                {
                    return Task.FromResult(ResultDto<bool>.Failure($"Invalid staff ID format: {staffId}."));
                }
            }
            return Task.FromResult(ResultDto<bool>.Failure("Staff ID cannot be null or empty."));
        }

        public async Task<ResultDto<IEnumerable<StaffScheduleDto>>> GetSchedulesByDoctorIdAsync(string doctorId)
        {
            // 1. Validate that the doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                return ResultDto<IEnumerable<StaffScheduleDto>>.Failure($"Doctor with ID {doctorId} not found.");
            }



            // 2. Retrieve the recurring schedules for the doctor
            var recurringSchedules = await _doctorRepository.GetSchedulesByDoctorIdAsync(doctorId);

            // 3. Map the recurring schedules to DTOs
            var doctorScheduleDtos = recurringSchedules.Select(schedule => new StaffScheduleDto
            (
                Id: schedule.Id,
                StaffId: schedule.DoctorId,
                StartDate: schedule.ShiftStart.Date,
                EndDate: schedule.ShiftEnd.Date,
                DaysOfWeek: GetDaysOfWeek(schedule.ShiftStart, schedule.ShiftEnd),
                StartTime: schedule.ShiftStart.TimeOfDay,
                EndTime: schedule.ShiftEnd.TimeOfDay
            ));

            return ResultDto<IEnumerable<StaffScheduleDto>>.SuccessResult(doctorScheduleDtos, "Doctor schedules retrieved successfully.");
        }
        public async Task<ResultDto<IEnumerable<StaffScheduleDto>>> GetSchedulesByNurseIdAsync(string nurseId)
        {
            // 1. Validate that the nurse exists
            var nurse = await _nurseRepository.GetByIdAsync(nurseId);
            if (nurse == null)
            {
                return ResultDto<IEnumerable<StaffScheduleDto>>.Failure($"Nurse with ID {nurseId} not found.");
            }



            // 2. Retrieve the recurring schedules for the nurse
            var recurringSchedules = await _nurseRepository.GetSchedulesByNurseIdAsync(nurseId);

            // 3. Map the recurring schedules to DTOs
            var nurseScheduleDtos = recurringSchedules.Select(schedule => new StaffScheduleDto
            (
                Id: schedule.Id,
                StaffId: schedule.NurseId,
                StartDate: schedule.ShiftStart.Date,
                EndDate: schedule.ShiftEnd.Date,
                DaysOfWeek: GetDaysOfWeek(schedule.ShiftStart, schedule.ShiftEnd),
                StartTime: schedule.ShiftStart.TimeOfDay,
                EndTime: schedule.ShiftEnd.TimeOfDay
            ));

            return ResultDto<IEnumerable<StaffScheduleDto>>.SuccessResult(nurseScheduleDtos, "Nurse schedules retrieved successfully.");
        }

        public Task<ResultDto<IEnumerable<StaffScheduleDto>>> GetSchedulesByStaffIdAsync(string staffId)
        {
            if (!string.IsNullOrEmpty(staffId)) 
            {
                if(staffId.StartsWith("DOC"))
                {
                    return GetSchedulesByDoctorIdAsync(staffId);
                }
                else if (staffId.StartsWith("NUR"))
                {
                    return GetSchedulesByNurseIdAsync(staffId);
                }
                else
                {
                    return Task.FromResult(ResultDto<IEnumerable<StaffScheduleDto>>.Failure($"Invalid staff ID format: {staffId}."));
                }
            }
            return Task.FromResult(ResultDto<IEnumerable<StaffScheduleDto>>.Failure("Staff ID cannot be null or empty."));
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
