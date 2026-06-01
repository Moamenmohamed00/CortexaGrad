using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IScheduleApplicationService
    {
        Task<ResultDto<bool>> GenerateRecurringSchedulesAsync(string doctorId, CreateRecurringScheduleDto command);
        Task<ResultDto<IEnumerable<RecurringScheduleDto>>> GetSchedulesByDoctorIdAsync(string doctorId);
    }
}
