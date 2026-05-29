using Cortexa.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cortexa.Application.Dtos.Auth
{
    public class UpdateDoctorUserRequestDto
    {
        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// 0 = Male, 1 = Female  (maps to Gender enum)
        /// </summary>
        public Gender Gender { get; set; }

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
        public ShiftType Shift { get; set; }

        public string Department { get; set; } = string.Empty;

        // ── Doctor-Only Fields ─────────────────────────────────────────
        public string Specialty { get; set; }

        /// <summary>
        /// 0 = Specialist, 1 = Consultant, 2 = Intern  (maps to DoctorRole enum)
        /// </summary>
        public DoctorRole DoctorRole { get; set; }

        public int ExperienceYears { get; set; }
    }
}
