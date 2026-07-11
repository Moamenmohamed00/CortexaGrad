using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record RemoveRoleFromUserCommand(string UserId, string RoleName) : IRequest<ResultDto<bool>>;

    public class RemoveRoleFromUserHandler : IRequestHandler<RemoveRoleFromUserCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;
        public RemoveRoleFromUserHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<bool>> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.RemoveRoleFromUser(request.UserId, request.RoleName);
            return result;
        }
    }


}
