using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using Cortexa.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Schedule.Commands
{
    public record CreateRecurringScheduleCommand(string DoctorId, CreateRecurringScheduleDto Request) : IRequest<ResultDto<bool>>;

    public class CreateRecurringScheduleCommandHandler : IRequestHandler<CreateRecurringScheduleCommand, ResultDto<bool>>
    {
        private readonly IScheduleApplicationService _scheduleService;
        public CreateRecurringScheduleCommandHandler(IScheduleApplicationService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        public async Task<ResultDto<bool>> Handle(CreateRecurringScheduleCommand request, CancellationToken cancellationToken)
        {
            return await _scheduleService.GenerateRecurringSchedulesAsync(request.DoctorId, request.Request);
        }
    }

    
}