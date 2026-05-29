using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Cortexa.Domain.Constants;
using Cortexa.Application.Dtos.Actors;

namespace Cortexa.Infrastructure.Identity
{


    /// <summary>
    /// Identity service backed by ASP.NET Identity (UserManager / SignInManager).
    /// Also creates domain entities (Doctor / Nurse) during registration.
    /// </summary>
    /// 
    public class IdentityService :
        Application.Common.Interfaces.IIdentityService,
        Application.Interfaces.Services.IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly ILogger<IdentityService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IEmailService emailService,
            ILogger<IdentityService> logger,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // ── Login ──────────────────────────────────────────────────────
        public async Task<ResultDto<AuthResponseDto>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed: No user with {Email}", email);
                return ResultDto<AuthResponseDto>.Failure("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                return ResultDto<AuthResponseDto>.Failure("Account is temporarily locked due to multiple failed login attempts.");
            }

            if (!result.Succeeded)
            {
                _logger.LogWarning("Authentication failed for {Email}", email);
                return ResultDto<AuthResponseDto>.Failure("Invalid email or password.");
            }

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            string? userIdInSystem = null;

            if (roles.Any())
            {
                // Unified validation and retrieval to eliminate duplicate DB hits
                if (roles.Contains(AppRoles.Doctor))
                {
                    var doctor = await _unitOfWork.Doctors.GetByEmailAsync(email);
                    if (doctor == null)
                    {
                        _logger.LogError("Data inconsistency: User {UserId} ({Email}) is in 'Doctor' role but no Doctor entity found", user.Id, email);
                        return ResultDto<AuthResponseDto>.Failure("Account data is corrupted. Please contact support.");
                    }
                    userIdInSystem = doctor.Id.ToString();
                }
                else if (roles.Contains(AppRoles.Nurse))
                {
                    var nurse = await _unitOfWork.Nurses.GetByEmailAsync(email);
                    if (nurse == null)
                    {
                        _logger.LogError("Data inconsistency: User {UserId} ({Email}) is in 'Nurse' role but no Nurse entity found", user.Id, email);
                        return ResultDto<AuthResponseDto>.Failure("Account data is corrupted. Please contact support.");
                    }
                    userIdInSystem = nurse.Id.ToString();
                }
                else if (roles.Contains(AppRoles.Admin))
                {
                    userIdInSystem = user.Id;
                }
                else
                {
                    _logger.LogError("Data inconsistency: User {UserId} ({Email}) has invalid role(s): {Roles}", user.Id, email, string.Join(", ", roles));
                    return ResultDto<AuthResponseDto>.Failure("Account data is corrupted. Please contact support.");
                }
            }
            else
            {
                _logger.LogWarning("Authentication warning: User {UserId} ({Email}) has no roles assigned", user.Id, email);
            }

            var token = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.FullName,
                user.Email!,
                roles);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                UserId = user.Id,
                Roles = roles,
                UserIdInSystem = userIdInSystem
            };

            _logger.LogInformation("User {UserId} ({Email}) authenticated successfully", user.Id, email);

            return ResultDto<AuthResponseDto>.SuccessResult(response, "Login successful.");
        }

        // ── Register ───────────────────────────────────────────────────
        public async Task<ResultDto<string>> AddUserAsync(AddUserRequestDto request)
        {
            // 1️⃣ Validate Role
            var role = request.Role?.Trim();

            if (string.IsNullOrWhiteSpace(role) ||
                (!role.Equals("Doctor", StringComparison.OrdinalIgnoreCase) &&
                 !role.Equals("Nurse", StringComparison.OrdinalIgnoreCase)))
            {
                return ResultDto<string>.Failure(
                    "Role must be either 'Doctor' or 'Nurse'.");
            }

            role = char.ToUpper(role[0]) + role[1..].ToLower();

            // 2️⃣ Create Identity User
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                NationalId = request.NationalId
                
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join("; ",
                    identityResult.Errors.Select(e => e.Description));

                _logger.LogWarning(
                    "Registration failed for {Email}: {Errors}",
                    request.Email, errors);

                return ResultDto<string>.Failure(errors);
            }

            // 3️⃣ Ensure Role Exists
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);

            // 4️⃣ Create Domain Entity
            var address = new Address(
                request.Street,
                request.City,
                request.State,
                request.ZipCode ?? string.Empty,
                string.Empty);

            if (role == "Doctor")
            {
                var doctor = new Doctor
                {
                    Name = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Address = address,
                    Specialty = request.Specialty ?? string.Empty,
                    Shift = request.Shift,
                    Role = request.DoctorRole ?? 0,
                    Department = request.Department,
                    ExperienceYears = request.ExperienceYears ?? 0,
                    NationalId = request.NationalId
                };

                await _unitOfWork.Doctors.AddAsync(doctor);
            }
            else
            {
                var nurse = new Nurse
                {
                    Name = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Address = address,
                    Shift = request.Shift,
                    Role = (request.NurseRole ?? 0),
                    Department = request.Department,
                    NationalId = request.NationalId
                };

                await _unitOfWork.Nurses.AddAsync(nurse);
            }

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            _logger.LogInformation(
                "{Role} {UserId} ({Email}) registered successfully",
                role, user.Id, request.Email);

            return ResultDto<string>.SuccessResult(
                user.Id,
                "Account created successfully.");
        }

        // ── Forgot Password (send OTP) ─────────────────────────────────
        public async Task<ResultDto<bool>> SendPasswordResetOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // ❗ Do NOT reveal whether the email exists
            if (user == null)
            {
                _logger.LogWarning(
                    "Password reset requested for non-existent email {Email}", email);

                return ResultDto<bool>.SuccessResult(
                    true,
                    "If an account with that email exists, a password reset OTP has been sent.");
            }

            var otp = await _userManager.GenerateTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider);

            var subject = "Cortexa — Password Reset OTP";

            var body =
        $@"Your password reset OTP is: {otp}

This code will expire shortly.
If you did not request a password reset, please ignore this email.";

            await _emailService.SendEmailAsync(user.Email!, subject, body);

            _logger.LogInformation("Password reset OTP sent to {Email}", email);

            return ResultDto<bool>.SuccessResult(
                true,
                "If an account with that email exists, a password reset OTP has been sent.");
        }

        // ── Reset Password with OTP ────────────────────────────────────
        public async Task<ResultDto<bool>> ResetPasswordWithOtpAsync(string email,string otp,string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning(
                    "Password reset attempted for non-existent email {Email}", email);

                return ResultDto<bool>.Failure(
                    "Invalid email or OTP.");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                otp);

            if (!isValid)
            {
                _logger.LogWarning("Invalid OTP for password reset: {Email}", email);

                return ResultDto<bool>.Failure(
                    "Invalid or expired OTP.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                resetToken,
                newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ",
                    result.Errors.Select(e => e.Description));

                _logger.LogWarning(
                    "Password reset failed for {Email}: {Errors}", email, errors);

                return ResultDto<bool>.Failure(errors);
            }

            _logger.LogInformation("Password reset successful for {Email}", email);

            return ResultDto<bool>.SuccessResult(
                true,
                "Password has been reset successfully.");
        }

        public async Task<ResultDto<DoctorDto>> UpdateDoctorDataAsync(string email, UpdateDoctorUserRequestDto request)
        {
            var doctor = await _unitOfWork.Doctors.GetByEmailAsync(email);
            if (doctor == null)
            {
                return ResultDto<DoctorDto>.Failure("Doctor not found.");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return ResultDto<DoctorDto>.Failure("Associated user account not found.");
            }

            var address = new Address(
                request.Street ?? doctor.Address.Street,
                request.City ?? doctor.Address.City,
                request.State ?? doctor.Address.State,
                request.ZipCode ?? doctor.Address.ZipCode ?? string.Empty,
                "Egypt"
                );

            // Update doctor properties
            doctor.Name = request.FullName ?? doctor.Name;
            doctor.PhoneNumber = request.PhoneNumber ?? doctor.PhoneNumber;
            doctor.DateOfBirth = request.DateOfBirth;
            doctor.Gender = request.Gender;
            doctor.NationalId = request.NationalId ?? doctor.NationalId;
            doctor.Address = address;
            doctor.Shift = request.Shift;
            doctor.Department = request.Department ?? doctor.Department;
            doctor.Specialty = request.Specialty ?? doctor.Specialty;
            doctor.Role = request.DoctorRole;
            doctor.ExperienceYears = request.ExperienceYears;
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Update Identity user properties if needed

                user.FullName = request.FullName ?? user.FullName;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.NationalId = request.NationalId ?? user.NationalId;
    
                var identityResult = await _userManager.UpdateAsync(user);
    
                if (!identityResult.Succeeded)
                {
                    var errors = string.Join("; ",
                        identityResult.Errors.Select(e => e.Description));
    
                    _logger.LogWarning(
                        "Failed to update Identity user for doctor {Email}: {Errors}",
                        email, errors);
    
                    return ResultDto<DoctorDto>.Failure("Failed to update associated user account: " + errors);
                }
    
                _logger.LogInformation("Doctor data updated successfully for {Email}", email);



            return ResultDto<DoctorDto>.SuccessResult(
                new DoctorDto
                (
                    Id : doctor.Id,
                    Name : doctor.Name,
                    Email : doctor.Email,
                    PhoneNumber : doctor.PhoneNumber,
                    DateOfBirth : doctor.DateOfBirth,
                    Gender : doctor.Gender,
                    NationalId : doctor.NationalId,
                    Address : new AddressDto
                    (
                       Street : doctor.Address.Street,
                       City : doctor.Address.City,
                       State : doctor.Address.State,
                       ZipCode : doctor.Address.ZipCode               
                    ),
                    Shift : doctor.Shift,
                    Department : doctor.Department,
                    Specialty : doctor.Specialty,
                    Role : doctor.Role,
                    ExperienceYears : doctor.ExperienceYears
                ),
                "Doctor data updated successfully.");
        }

        public async Task<ResultDto<NurseDto>> UpdateNurseDataAsync(string email, UpdateNurseUserRequestDto request)
        {
            var nurse = await _unitOfWork.Nurses.GetByEmailAsync(email);
            if (nurse == null)
            {
                return ResultDto<NurseDto>.Failure("Nurse not found.");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { 
                return ResultDto<NurseDto>.Failure("Associated user account not found.");
            }

            var address = new Address(
                request.Street ?? nurse.Address.Street,
                request.City ?? nurse.Address.City,
                request.State ?? nurse.Address.State,
                request.ZipCode ?? nurse.Address.ZipCode ?? string.Empty,
                "Egypt"
                );

            // Update nurse properties

            nurse.Name = request.FullName ?? nurse.Name;
            nurse.PhoneNumber = request.PhoneNumber ?? nurse.PhoneNumber;
            nurse.DateOfBirth = request.DateOfBirth;
            nurse.Gender = request.Gender;
            nurse.NationalId = request.NationalId ?? nurse.NationalId;
            nurse.Address = address;
            nurse.Shift = request.Shift;
            nurse.Department = request.Department ?? nurse.Department;
            nurse.Role = request.Role;
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Update Identity user properties if needed

                user.FullName = request.FullName ?? user.FullName;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.NationalId = request.NationalId ?? user.NationalId;
    
                var identityResult = await _userManager.UpdateAsync(user);
    
                if (!identityResult.Succeeded)
                {
                    var errors = string.Join("; ",
                        identityResult.Errors.Select(e => e.Description));
    
                    _logger.LogWarning(
                        "Failed to update Identity user for nurse {Email}: {Errors}",
                        email, errors);
    
                    return ResultDto<NurseDto>.Failure("Failed to update associated user account: " + errors);
                }
    
                _logger.LogInformation("Nurse data updated successfully for {Email}", email);



            return ResultDto<NurseDto>.SuccessResult(
                new NurseDto
                (
                    Id : nurse.Id,
                    Name : nurse.Name,
                    Email : nurse.Email,
                    PhoneNumber : nurse.PhoneNumber,
                    DateOfBirth : nurse.DateOfBirth,
                    Gender : nurse.Gender,
                    NationalId : nurse.NationalId,
                    Address : new AddressDto
                    (
                       Street : nurse.Address.Street,
                       City : nurse.Address.City,
                       State : nurse.Address.State,
                       ZipCode : nurse.Address.ZipCode               
                    ),
                    Shift : nurse.Shift,
                    Department : nurse.Department,
                    Role : nurse.Role
                ),
                "Nurse data updated successfully.");
        }
    }
}
