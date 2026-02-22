using System.ComponentModel.DataAnnotations;

namespace Cortexa.Application.Dtos.Auth
{
    /// <summary>
    /// Registration request. The Role field ("Doctor" or "Nurse") determines
    /// which role-specific fields are required.
    /// </summary>
    public class RegisterRequestDto
    {
        // ── Auth Credentials ───────────────────────────────────────────
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Must be "Doctor" or "Nurse".
        /// </summary>
        [Required]
        public string Role { get; set; } = string.Empty;

        // ── Common AppUser Fields ──────────────────────────────────────
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// 0 = Male, 1 = Female  (maps to Gender enum)
        /// </summary>
        public int Gender { get; set; }

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [StringLength(14, MinimumLength = 14,
          ErrorMessage = "الرقم القومي يجب أن يكون 14 رقمًا")]
        [RegularExpression(@"^\d{14}$",
          ErrorMessage = "الرقم القومي يجب أن يحتوي على أرقام فقط")]
        public string NationalId { get; set; } = string.Empty;


        // Address
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string? ZipCode { get; set; }

        // ── Shared Staff Fields ────────────────────────────────────────
        /// <summary>
        /// 0 = Morning, 1 = Evening, 2 = Night  (maps to ShiftType enum)
        /// </summary>
        public int Shift { get; set; }

        public string Department { get; set; } = string.Empty;

        // ── Doctor-Only Fields ─────────────────────────────────────────
        public string? Specialty { get; set; }

        /// <summary>
        /// 0 = Specialist, 1 = Consultant, 2 = Intern  (maps to DoctorRole enum)
        /// </summary>
        public int? DoctorRole { get; set; }

        public int? ExperienceYears { get; set; }

        // ── Nurse-Only Fields ──────────────────────────────────────────
        /// <summary>
        /// 0 = Staff, 1 = HeadNurse  (maps to NurseRole enum)
        /// </summary>
        public int? NurseRole { get; set; }
    }
}
