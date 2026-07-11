using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;

namespace Cortexa.Application.Common.Interfaces
{
    /// <summary>
    /// Legacy identity interface — preserved for backward compatibility.
    /// New code should use Cortexa.Application.Interfaces.Services.IIdentityService instead.
    /// </summary>
    public interface IIdentityService
    {
        Task<ResultDto<AuthResponseDto>> LoginAsync(string email, string password);
    }
}
