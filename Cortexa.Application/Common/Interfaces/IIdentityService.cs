using Cortexa.Application.Dtos.Auth;

namespace Cortexa.Application.Common.Interfaces
{
    /// <summary>
    /// Legacy identity interface — preserved for backward compatibility.
    /// New code should use Cortexa.Application.Interfaces.Services.IIdentityService instead.
    /// </summary>
    public interface IIdentityService
    {
        Task<AuthResponseDto> LoginAsync(string email, string password);
    }
}
