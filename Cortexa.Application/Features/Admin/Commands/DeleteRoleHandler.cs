using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record DeleteRoleCommand(string RoleId) : IRequest<ResultDto<bool>>;

    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;

        public DeleteRoleHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.DeleteRole(request.RoleId);
            return result;
        }
    }
}
