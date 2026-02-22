using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Auth
{
    // ── Command ────────────────────────────────────────────────────────
    public record RegisterCommand(RegisterRequestDto Request) : IRequest<ResultDto<string>>;

    // ── Handler ────────────────────────────────────────────────────────
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResultDto<string>>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RegisterAsync(request.Request);
        }
    }
}
