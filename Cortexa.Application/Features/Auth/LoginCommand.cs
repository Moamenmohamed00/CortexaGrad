using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Auth
{
    // ── Command ────────────────────────────────────────────────────────
    public record LoginCommand(string Email, string Password) : IRequest<ResultDto< AuthResponseDto>>;

    // ── Handler ────────────────────────────────────────────────────────
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResultDto<AuthResponseDto>>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginAsync(request.Email, request.Password);
        }
    }
}
