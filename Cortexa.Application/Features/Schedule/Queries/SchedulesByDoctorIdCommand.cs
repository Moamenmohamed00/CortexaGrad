using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Schedule.Queries
{
    public record SchedulesByStaffIdCommand(string StaffId) : IRequest<ResultDto<IEnumerable<StaffScheduleDto>>>;
    public class SchedulesByStaffIdCommandHandler : IRequestHandler<SchedulesByStaffIdCommand, ResultDto<IEnumerable<StaffScheduleDto>>>
    {
        private readonly IScheduleApplicationService _scheduleService;
        public SchedulesByStaffIdCommandHandler(IScheduleApplicationService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        public async Task<ResultDto<IEnumerable<StaffScheduleDto>>> Handle(SchedulesByStaffIdCommand request, CancellationToken cancellationToken)
        {
            return await _scheduleService.GetSchedulesByStaffIdAsync(request.StaffId);
        }

    }
}