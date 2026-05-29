using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using MediatR;
using Cortexa.Application.Interfaces.Services;

namespace Cortexa.Application.Features.Auth
{
    public record UpdateNurseUserCommand(UpdateNurseUserRequestDto Request) : IRequest<ResultDto<NurseDto>>;

    public class UpdateNurseUserCommandHandler : IRequestHandler<UpdateNurseUserCommand, ResultDto<NurseDto>>
    {
        private readonly IIdentityService _identityService;

        public UpdateNurseUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<NurseDto>> Handle(UpdateNurseUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.UpdateNurseDataAsync(request.Request);
        }
    }
}
