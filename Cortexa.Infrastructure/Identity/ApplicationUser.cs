using Microsoft.AspNetCore.Identity;

namespace Cortexa.Infrastructure.Identity
{
    /// <summary>
    /// ASP.NET Identity user for authentication and authorization.
    /// This is separate from the domain AppUser hierarchy (Patient, Doctor, Nurse)
    /// and serves only as the authentication credential store.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Full display name of the user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;
    }
}
