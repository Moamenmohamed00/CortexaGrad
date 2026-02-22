using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Enums;
using Cortexa.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.Persistence.Seeding
{
    /// <summary>
    /// Seeds default staff users: Doctors, Nurses, and sample Patients.
    /// </summary>
    public class DefaultUsersSeeder
    {
        private readonly CortexaDbContext _context;
        private readonly ILogger<DefaultUsersSeeder> _logger;

        public DefaultUsersSeeder(CortexaDbContext context, ILogger<DefaultUsersSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (await _context.Doctors.AnyAsync())
            {
                _logger.LogInformation("Default users already seeded. Skipping.");
                return;
            }

            _logger.LogInformation("Seeding default users (Doctors, Nurses, Patients)...");

            var doctors = CreateDoctors();
            var nurses = CreateNurses();
            var patients = CreatePatients();

            await _context.Doctors.AddRangeAsync(doctors);
            await _context.Nurses.AddRangeAsync(nurses);
            await _context.Patients.AddRangeAsync(patients);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Seeded {DoctorCount} doctors, {NurseCount} nurses, {PatientCount} patients.",
                doctors.Count, nurses.Count, patients.Count);
        }

        // ── Doctors ────────────────────────────────────────────────────
        private List<Doctor> CreateDoctors()
        {
            return new List<Doctor>
    {
        new()
        {
            Name = "Dr. Ahmed Hassan",
            Email = "ahmed.hassan@cortexa.com",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1975, 3, 15),
            Address = new Address("10 Qasr Al-Aini St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-0001",
            Specialty = "Internal Medicine",
            Shift = ShiftType.Morning,
            Role = DoctorRole.Consultant,
            Department = "Internal Medicine",
            ExperienceYears = 20,
            NationalId = "27503151500123"
        },
        new()
        {
            Name = "Dr. Fatma Ali",
            Email = "fatma.ali@cortexa.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1982, 7, 22),
            Address = new Address("5 Tahrir St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-0002",
            Specialty = "Cardiology",
            Shift = ShiftType.Morning,
            Role = DoctorRole.Specialist,
            Department = "Cardiology",
            ExperienceYears = 14,
            NationalId = "28207221500987"
        },
        new()
        {
            Name = "Dr. Omar Khaled",
            Email = "omar.khaled@cortexa.com",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1988, 11, 5),
            Address = new Address("22 El-Haram St", "Giza", "Giza Governorate", "12511", "Egypt"),
            PhoneNumber = "+20-100-555-0003",
            Specialty = "Emergency Medicine",
            Shift = ShiftType.Evening,
            Role = DoctorRole.Specialist,
            Department = "Emergency",
            ExperienceYears = 8,
            NationalId = "28811051500456"
        },
        new()
        {
            Name = "Dr. Nour Sayed",
            Email = "nour.sayed@cortexa.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1995, 1, 10),
            Address = new Address("3 Nile Corniche", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-0004",
            Specialty = "General Surgery",
            Shift = ShiftType.Night,
            Role = DoctorRole.Intern,
            Department = "Surgery",
            ExperienceYears = 2,
            NationalId = "29501101500654"
        }
    };
        }

        // ── Nurses ─────────────────────────────────────────────────────
        private List<Nurse> CreateNurses()
        {
            return new List<Nurse>
    {
        new()
        {
            Name = "Mona Abdel-Fattah",
            Email = "mona.abdel@cortexa.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1985, 4, 20),
            Address = new Address("8 Ramses St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-1001",
            Shift = ShiftType.Morning,
            Role = NurseRole.HeadNurse,
            Department = "Internal Medicine",
            NationalId = "28504201500111"
        },
        new()
        {
            Name = "Heba Mohamed",
            Email = "heba.mohamed@cortexa.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1990, 8, 12),
            Address = new Address("15 Salah Salem St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-1002",
            Shift = ShiftType.Morning,
            Role = NurseRole.Staff,
            Department = "Internal Medicine",
            NationalId = "29008121500222"
        },
        new()
        {
            Name = "Youssef Ibrahim",
            Email = "youssef.ibrahim@cortexa.com",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1992, 2, 28),
            Address = new Address("45 Abbas El-Akkad", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-1003",
            Shift = ShiftType.Evening,
            Role = NurseRole.Staff,
            Department = "Emergency",
            NationalId = "29202281500333"
        },
        new()
        {
            Name = "Sara Gamal",
            Email = "sara.gamal@cortexa.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1988, 6, 15),
            Address = new Address("12 Makram Ebeid", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-100-555-1004",
            Shift = ShiftType.Night,
            Role = NurseRole.Staff,
            Department = "ICU",
            NationalId = "28806151500444"
        }
    };
        }

        // ── Patients ───────────────────────────────────────────────────
        private List<Patient> CreatePatients()
        {
            return new List<Patient>
    {
        new()
        {
            Name = "Mahmoud Saad",
            Email = "mahmoud.saad@example.com",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1960, 5, 10),
            Address = new Address("33 El-Moez St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-101-000-0001",
            FileNumber = "PAT-2025-0001",
            DiagnosisSummary = "Hypertension, Type 2 Diabetes Mellitus",
            BloodType = BloodType.APositive,
            NationalId = "26005101500555"
        },
        new()
        {
            Name = "Aisha Mostafa",
            Email = "aisha.mostafa@example.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1978, 9, 25),
            Address = new Address("7 Shehab St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-101-000-0002",
            FileNumber = "PAT-2025-0002",
            DiagnosisSummary = "Acute Myocardial Infarction",
            BloodType = BloodType.OPositive,
            NationalId = "27809251500666"
        },
        new()
        {
            Name = "Khaled Tarek",
            Email = "khaled.tarek@example.com",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1945, 12, 3),
            Address = new Address("19 Sharia 26 July", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-101-000-0003",
            FileNumber = "PAT-2025-0003",
            DiagnosisSummary = "COPD Exacerbation, Pneumonia",
            BloodType = BloodType.BPositive,
            NationalId = "24512031500777"
        },
        new()
        {
            Name = "Layla Abdel-Rahman",
            Email = "layla.rahman@example.com",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1990, 3, 18),
            Address = new Address("2 Gesr El-Suez St", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-101-000-0004",
            FileNumber = "PAT-2025-0004",
            DiagnosisSummary = "Appendicitis – Post-Appendectomy",
            BloodType = BloodType.ABPositive,
            NationalId = "29003181500888"
        },
        new()
        {
            Name = "Hassan El-Naggar",
            Email = "hassan.naggar@example.com",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(1955, 7, 30),
            Address = new Address("56 Corniche El-Nil", "Cairo", "Cairo Governorate", "11511", "Egypt"),
            PhoneNumber = "+20-101-000-0005",
            FileNumber = "PAT-2025-0005",
            DiagnosisSummary = "Chronic Kidney Disease Stage 4",
            BloodType = BloodType.ONegative,
            NationalId = "25507301500999"
        }
    };
        }
    }
    }

