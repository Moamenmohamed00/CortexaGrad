using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cortexa.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cortexa.Infrastructure.Identity
{
    /// <summary>
    /// Generates JWT tokens using JwtSecurityTokenHandler.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(string userId, string userName, string email, IEnumerable<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signingCredentials = new SigningCredentials(
                signingKey, SecurityAlgorithms.HmacSha256);

            // Build claims
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.Name, userName),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                // Map to ClaimTypes for CurrentUserService compatibility
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Name, userName),
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
