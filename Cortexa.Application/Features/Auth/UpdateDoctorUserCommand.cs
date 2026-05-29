using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Cortexa.Application.Features.Auth
{
    public record UpdateDoctorUserCommand(string Email, UpdateDoctorUserRequestDto Request) : IRequest<ResultDto<DoctorDto>>;

    public class UpdateDoctorUserCommandHandler : IRequestHandler<UpdateDoctorUserCommand, ResultDto<DoctorDto>>
    {
        private readonly IIdentityService _identityService;

        public UpdateDoctorUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<DoctorDto>> Handle(UpdateDoctorUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.UpdateDoctorDataAsync(request.Email, request.Request);
        }
    }
}
 
