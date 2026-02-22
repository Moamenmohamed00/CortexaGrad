using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Auth
{
    // ── Command ────────────────────────────────────────────────────────
    public record ForgotPasswordCommand(string Email) : IRequest<ResultDto<bool>>;

    // ── Handler ────────────────────────────────────────────────────────
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ResultDto<bool>>
    {
        private readonly IIdentityService _identityService;

        public ForgotPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.SendPasswordResetOtpAsync(request.Email);
        }
    }
}
