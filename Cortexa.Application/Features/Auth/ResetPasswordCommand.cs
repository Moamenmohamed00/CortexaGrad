using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Services;
using MediatR;

namespace Cortexa.Application.Features.Auth
{
    // ── Command ────────────────────────────────────────────────────────
    public record ResetPasswordCommand(string Email, string Otp, string NewPassword) : IRequest<ResultDto<bool>>;

    // ── Handler ────────────────────────────────────────────────────────
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResultDto<bool>>
    {
        private readonly IIdentityService _identityService;

        public ResetPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResultDto<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.ResetPasswordWithOtpAsync(request.Email, request.Otp, request.NewPassword);
        }
    }
}
