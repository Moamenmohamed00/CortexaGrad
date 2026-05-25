using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace Cortexa.Application.Features.Admin.Commands
{

    public record AssignRoleToUserCommand(string UserId, string RoleName) : IRequest<ResultDto<bool>>;

    public class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;
        public AssignRoleToUserHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResultDto<bool>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.AssignRoleToUser(request.UserId, request.RoleName);
            return result;
        }
    }

}
