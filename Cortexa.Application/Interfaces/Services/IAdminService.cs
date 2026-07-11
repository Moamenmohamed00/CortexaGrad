using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Admin;
using Cortexa.Application.Dtos.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<ResultDto<bool>> CreateAdmin(string Email, string Password, string Username, string FullName);

        Task<ResultDto<bool>> DeleteRole(string RoleId);

        Task<ResultDto<bool>> CreateRole(string RoleName);

        Task<ResultDto<bool>> AssignRoleToUser(string UserId, string RoleName);

        Task<ResultDto<bool>> RemoveRoleFromUser(string UserId, string RoleName);

        Task<ResultDto<List<RoleDto>>> GetAllRoles();

        Task<ResultDto<List<UserRoleDto>>> GetAllUsersWithRoles();

        Task <ResultDto<bool>> DeleteUser(string UserId);

        Task <ResultDto<bool>> ToggleUserStatus(string UserId);

        Task <ResultDto<bool>> ForceResetPassword(string UserId, string NewPassword);

        

    }
}
