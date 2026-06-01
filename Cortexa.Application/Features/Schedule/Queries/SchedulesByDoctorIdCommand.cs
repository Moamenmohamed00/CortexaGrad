using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Schedule.Queries
{
    public record SchedulesByDoctorIdCommand(string DoctorId) : IRequest<ResultDto<IEnumerable<RecurringScheduleDto>>>;
    public class SchedulesByDoctorIdCommandHandler : IRequestHandler<SchedulesByDoctorIdCommand, ResultDto<IEnumerable<RecurringScheduleDto>>>
    {
        private readonly IScheduleApplicationService _scheduleService;
        public SchedulesByDoctorIdCommandHandler(IScheduleApplicationService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        public async Task<ResultDto<IEnumerable<RecurringScheduleDto>>> Handle(SchedulesByDoctorIdCommand request, CancellationToken cancellationToken)
        {
            return await _scheduleService.GetSchedulesByDoctorIdAsync(request.DoctorId);
        }

    }
}