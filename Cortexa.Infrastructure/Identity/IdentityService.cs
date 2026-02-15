using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Dtos.Auth;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.Identity
{
    /// <summary>
    /// Identity service backed by ASP.NET Identity (UserManager / SignInManager).
    /// Also creates domain entities (Doctor / Nurse) during registration.
    /// </summary>
    public class IdentityService :
        Application.Common.Interfaces.IIdentityService,
        Application.Interfaces.Services.IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IEmailService emailService,
            IApplicationDbContext dbContext,
            ILogger<IdentityService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _dbContext = dbContext;
            _logger = logger;
        }

        // ── Login ──────────────────────────────────────────────────────
        public async Task<AuthResponseDto> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed: No user found with email {Email}", email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Authentication failed: Invalid password for {Email}", email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var token = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.FullName,
                user.Email!,
                roles);

            _logger.LogInformation("User {UserId} ({Email}) authenticated successfully", user.Id, email);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                UserId = user.Id,
                Roles = roles
            };
        }

        // ── Register ───────────────────────────────────────────────────
        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            // Validate and normalize role (case-insensitive)
            var role = request.Role?.Trim();
            if (string.IsNullOrEmpty(role) ||
                !role.Equals("Doctor", StringComparison.OrdinalIgnoreCase) &&
                !role.Equals("Nurse", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Role must be 'Doctor' or 'Nurse'.");
            }

            // Normalize to title case for Identity role name
            role = char.ToUpper(role[0]) + role[1..].ToLower();

            // 1. Create the Identity user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Registration failed for {Email}: {Errors}", request.Email, errors);
                throw new InvalidOperationException($"Registration failed: {errors}");
            }

            // 2. Ensure the role exists, then assign it
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);

            // 3. Create the domain entity
            var address = new Address(
                request.Street, request.City, request.State,
                request.ZipCode ?? string.Empty, string.Empty);

            if (role == "Doctor")
            {
                var doctor = new Doctor
                {
                    Name = request.FullName,
                    Email = request.Email,
                    PhoneNumbers = request.PhoneNumbers,
                    DateOfBirth = request.DateOfBirth,
                    Gender = (Gender)request.Gender,
                    Address = address,
                    Specialty = request.Specialty ?? string.Empty,
                    Shift = (ShiftType)request.Shift,
                    Role = (DoctorRole)(request.DoctorRole ?? 0),
                    Department = request.Department,
                    ExperienceYears = request.ExperienceYears ?? 0
                };
                _dbContext.Doctors.Add(doctor);
            }
            else // Nurse
            {
                var nurse = new Nurse
                {
                    Name = request.FullName,
                    Email = request.Email,
                    PhoneNumbers = request.PhoneNumbers,
                    DateOfBirth = request.DateOfBirth,
                    Gender = (Gender)request.Gender,
                    Address = address,
                    Shift = (ShiftType)request.Shift,
                    Role = (NurseRole)(request.NurseRole ?? 0),
                    Department = request.Department
                };
                _dbContext.Nurses.Add(nurse);
            }

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            _logger.LogInformation("{Role} {UserId} ({Email}) registered successfully", role, user.Id, request.Email);
            return user.Id;
        }

        // ── Forgot Password (send OTP) ─────────────────────────────────
        public async Task<bool> SendPasswordResetOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existent email {Email}", email);
                return true; // Don't reveal whether the email exists
            }

            var otp = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

            var subject = "Cortexa — Password Reset OTP";
            var body = $"Your reset OTP is: {otp}\n\nThis code will expire shortly. If you did not request a password reset, please ignore this email.";
            await _emailService.SendEmailAsync(user.Email!, subject, body);

            _logger.LogInformation("Password reset OTP sent to {Email}", email);
            return true;
        }

        // ── Reset Password with OTP ────────────────────────────────────
        public async Task<bool> ResetPasswordWithOtpAsync(string email, string otp, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email {Email}", email);
                return false;
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, TokenOptions.DefaultEmailProvider, otp);

            if (!isValid)
            {
                _logger.LogWarning("Invalid OTP for password reset: {Email}", email);
                return false;
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed for {Email}: {Errors}", email, errors);
                return false;
            }

            _logger.LogInformation("Password reset successful for {Email}", email);
            return true;
        }
    }
}
