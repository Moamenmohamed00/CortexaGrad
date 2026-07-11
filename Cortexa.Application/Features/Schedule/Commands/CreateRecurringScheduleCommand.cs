using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Schedule;
using Cortexa.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Schedule.Commands
{
    public record CreateStaffScheduleCommand(string StaffId, CreateStaffScheduleDto Request) : IRequest<ResultDto<bool>>;

    public class CreateStaffScheduleCommandHandler : IRequestHandler<CreateStaffScheduleCommand, ResultDto<bool>>
    {
        private readonly IScheduleApplicationService _scheduleService;
        public CreateStaffScheduleCommandHandler(IScheduleApplicationService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        public async Task<ResultDto<bool>> Handle(CreateStaffScheduleCommand request, CancellationToken cancellationToken)
        {
            return await _scheduleService.GenerateStaffSchedulesAsync(request.StaffId, request.Request);
        }
    }

    
}