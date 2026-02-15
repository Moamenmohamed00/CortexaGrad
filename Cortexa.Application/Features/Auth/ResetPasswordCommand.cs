using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Auth
{
    // ── Command ────────────────────────────────────────────────────────
    public record ResetPasswordCommand(string Email, string Otp, string NewPassword) : IRequest<bool>;

    // ── Handler ────────────────────────────────────────────────────────
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public ResetPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.ResetPasswordWithOtpAsync(request.Email, request.Otp, request.NewPassword);
        }
    }
}
