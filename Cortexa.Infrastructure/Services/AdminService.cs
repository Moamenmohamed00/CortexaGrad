using Azure.Core;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Cortexa.Application.Dtos.Admin;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Cortexa.Infrastructure.Services
{
    internal class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IImageService _imageService;

        public AdminService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IImageService imageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _imageService = imageService;
        }

        public async Task<ResultDto<bool>> AssignRoleToUser(string UserId, string RoleName)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return new ResultDto<bool> { Data = false, Success = false, Message = "User not found." };

            // 2. Check if role exists

            var roleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (!roleExists) return new ResultDto<bool> { Data = false, Success = false, Message = "Role not found." };
            // 3. Check if user already has this role to avoid duplicate entry errors

            var isInRole = await _userManager.IsInRoleAsync(user, RoleName);
            if (isInRole) return new ResultDto<bool> { Data = true, Success = true, Message = "Role already assigned." };

            // 4. Assign role to user
            var result = await _userManager.AddToRoleAsync(user, RoleName);
            if (!result.Succeeded) return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to assign role." };
            return new ResultDto<bool> { Data = true, Success = true, Message = "Role assigned successfully." };
        }

        public async Task<ResultDto<bool>> CreateAdmin(string Email, string Password, string Username, string FullName)
        {
            var userExists = await _userManager.FindByEmailAsync(Email);
            if (userExists != null) return new ResultDto<bool> { Data = false, Success = false, Message = "User already exists." };
            var newAdmin = new ApplicationUser
            {
                FullName= FullName,
                UserName = Username,
                Email = Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newAdmin, Password);
            if (!result.Succeeded) return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to create user." };

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(newAdmin, "Admin");
            return new ResultDto<bool> { Data = true, Success = true, Message = "Admin user created successfully." };
        }

        public async Task<ResultDto<bool>> CreateRole(string RoleName)
        {
            if (await _roleManager.RoleExistsAsync(RoleName))
            {
                return new ResultDto<bool> { Data = false, Success = false, Message = "Role already exists." };
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
            if (!result.Succeeded) return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to create role." };

            return new ResultDto<bool> { Data = true, Success = true, Message = "Role created successfully." };
        }

        public async Task<ResultDto<bool>> DeleteRole(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null) return new ResultDto<bool> { Data = false, Success = false, Message = "Role not found." };
            await _roleManager.DeleteAsync(role);
            return new ResultDto<bool> { Data = true, Success = true, Message = "Role deleted successfully." };
        }

        public async Task<ResultDto<List<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto(r.Id, r.Name))
                .ToListAsync();
            return new ResultDto<List<RoleDto>> { Data = roles, Success = true, Message = "Roles retrieved successfully." };
        }

        public async Task<ResultDto<bool>> RemoveRoleFromUser(string UserId, string RoleName)
        {


            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return new ResultDto<bool> { Data = false, Success = false, Message = "User not found." };

            // 2. Check if role exists

            var roleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (!roleExists) return new ResultDto<bool> { Data = false, Success = false, Message = "Role not found." };

            // 3. Check if user already has this role to avoid duplicate entry errors

            var isInRole = await _userManager.IsInRoleAsync(user, RoleName);
            if (!isInRole)
                return new ResultDto<bool> { Data = false, Success = false, Message = "Role already Not assigned." };

            // 4. Assign role to user
            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            if (!result.Succeeded) return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to Delete role From User." };
            return new ResultDto<bool> { Data = true, Success = true, Message = "Role Deleted From User successfully." };
        }

        public async Task<ResultDto<List<UserRoleDto>>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null || users.Count == 0) return new ResultDto<List<UserRoleDto>> { Data = null, Success = false, Message = "No users found." };
            var userRoles = new List<UserRoleDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleDto(user.Id, user.UserName, roles.ToList())
                );
            }
            return new ResultDto<List<UserRoleDto>> { Data = userRoles, Success = true, Message = "Users with roles retrieved successfully." };

        }

        public Task<ResultDto<bool>> DeleteUser(string UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<bool>> ToggleUserStatus(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return new ResultDto<bool> { Data = false, Success = false, Message = "User not found." };

            user.LockoutEnabled = !user.LockoutEnabled;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to toggle user status." };

            return new ResultDto<bool> { Data = true, Success = true, Message = "User status toggled successfully." };
        }

        public async Task<ResultDto<bool>> ForceResetPassword(string UserId, string NewPassword)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return new ResultDto<bool> { Data = false, Success = false, Message = "User not found." };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, NewPassword);
            if (!result.Succeeded) return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to reset password." };

            return new ResultDto<bool> { Data = true, Success = true, Message = "Password reset successfully." };
        }

        public async Task<ResultDto<string>> UploadPhotoAsync(byte[] photo, string fileName)
        {
            using var ms = new MemoryStream(photo);
            var result = await _imageService.UploadImageAsync(ms, fileName);

            if (string.IsNullOrEmpty(result.Url) || string.IsNullOrEmpty(result.PublicId))
            {
                return new ResultDto<string>
                {
                    Data = null,
                    Success = false,
                    Message = "Failed to upload image."
                };
            }

            return new ResultDto<string>
            {
                Data = result.Url,
                Success = true,
                Message = "Image uploaded successfully."
            };
        }
    }
}
