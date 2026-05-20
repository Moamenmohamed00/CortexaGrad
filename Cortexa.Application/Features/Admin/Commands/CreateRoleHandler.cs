using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record CreateRoleCommand(string RoleName) : IRequest<ResultDto<bool>>;

    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;
        public CreateRoleHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.CreateRole(request.RoleName);
            return result;
        }
    }
}
