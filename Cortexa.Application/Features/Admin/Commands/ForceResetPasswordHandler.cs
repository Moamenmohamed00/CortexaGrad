using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record ForceResetPasswordCommand(string UserId, string NewPassword) : IRequest<ResultDto<bool>>;

    public class ForceResetPasswordHandler : IRequestHandler<ForceResetPasswordCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;
        public ForceResetPasswordHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<bool>> Handle(ForceResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.ForceResetPassword(request.UserId, request.NewPassword);
            return result;
        }
    }
}