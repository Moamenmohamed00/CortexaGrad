using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record ToggleUserStatusCommand(string Id) : IRequest<ResultDto<bool>>;

    public class ToggleUserStatusHandler : IRequestHandler<ToggleUserStatusCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;
        public ToggleUserStatusHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<bool>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.ToggleUserStatus(request.Id);
            return result;
        }
    }

}