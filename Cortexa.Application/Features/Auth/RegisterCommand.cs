using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Auth
{
    // ── Command ────────────────────────────────────────────────────────
    public record AddUserCommand(AddUserRequestDto Request) : IRequest<ResultDto<string>>;

    // ── Handler ────────────────────────────────────────────────────────
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ResultDto<string>>
    {
        private readonly IIdentityService _identityService;

        public AddUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.AddUserAsync(request.Request);
        }
    }
}
