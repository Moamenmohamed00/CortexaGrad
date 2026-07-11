using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IScheduleApplicationService
    {
        Task<ResultDto<bool>> GenerateStaffSchedulesAsync(string staffId, CreateStaffScheduleDto command);
        Task<ResultDto<IEnumerable<StaffScheduleDto>>> GetSchedulesByStaffIdAsync(string staffId);
    }
}
