using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Cortexa.Application.Dtos.Admin;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Application.Dtos.Core;

namespace Cortexa.Application.Features.Admin.Queries
{
    public record GetRolesQuery() : IRequest<ResultDto<List<RoleDto>>>;

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, ResultDto<List<RoleDto>>>
    {
        private readonly IAdminService _adminService;

        public GetRolesQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<ResultDto<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var rolesResult = await _adminService.GetAllRoles();
            if (!rolesResult.Success)
            {
                // Handle error scenario, e.g., throw an exception or return an empty list
                return new ResultDto<List<RoleDto>> { Data = new List<RoleDto>(), Success = false, Message = "Failed to retrieve roles." };
            }

            return rolesResult;
        }
    }

}
