using MediatR;
using Cortexa.Application.Dtos.Admin;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Application.Dtos.Core;

namespace Cortexa.Application.Features.Admin.Queries
{
    public record GetUsersWithRolesQuery() : IRequest<ResultDto<List<UserRoleDto>>>;

    public class GetUsersWithRolesQueryHandler : IRequestHandler<GetUsersWithRolesQuery, ResultDto<List<UserRoleDto>>>
    {
        private readonly IAdminService _adminService;
        public GetUsersWithRolesQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<List<UserRoleDto>>> Handle(GetUsersWithRolesQuery request, CancellationToken cancellationToken)
        {
            var usersWithRolesResult = await _adminService.GetAllUsersWithRoles();
            return usersWithRolesResult;
        }

    }
}
