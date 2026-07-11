using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record CreateAdminCommand(string Email, string Password, string Username, string FullName) : IRequest<ResultDto<bool>>;
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, ResultDto<bool>>
    {
        private readonly IAdminService _adminService;
        public CreateAdminCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<ResultDto<bool>> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            var result = await _adminService.CreateAdmin(request.Email, request.Password, request.Username, request.FullName);
            return result;
        }
            
    }
}
