using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortexa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Origian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiagnosisSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BloodType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeSources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeSources_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RAGQueries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QueryText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoreTrust = table.Column<float>(type: "real", nullable: false),
                    RelevanceLevel = table.Column<int>(type: "int", nullable: false),
                    QueryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratedResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAGQueries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAGQueries_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAGQueries_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InitialDiagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DischargeSummary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DischargeDisposition = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlertMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BedNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentAdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beds_Admissions_CurrentAdmissionId",
                        column: x => x.CurrentAdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Beds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Complaint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentIllness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChronicDisease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneticDisease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialHabits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClinicalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseHistories_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseHistories_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CultureType = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sensitivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cultures_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cultures_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cultures_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluidBalances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount_ML = table.Column<int>(type: "int", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluidBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluidBalances_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluidBalances_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Findings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imagings_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Imagings_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterventionProcedures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    InsertionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionProcedures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterventionProcedures_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterventionProcedures_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabOrders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabOrders_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabOrders_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Dose = table.Column<int>(type: "int", nullable: false),
                    DoseUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Route = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medications_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NursingNotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoteText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursingNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NursingNotes_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NursingNotes_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalExaminations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    BloodPressure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pulse = table.Column<int>(type: "int", nullable: false),
                    RespRate = table.Column<int>(type: "int", nullable: false),
                    EyeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkinStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LipsStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeartExam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbdomenExam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalExamination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalExaminations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalExaminations_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhysicalExaminations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VitalSigns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    HeartRate = table.Column<int>(type: "int", nullable: false),
                    RespRate = table.Column<int>(type: "int", nullable: false),
                    BP_Systolic = table.Column<int>(type: "int", nullable: false),
                    BP_Diastolic = table.Column<int>(type: "int", nullable: false),
                    PulseOxy = table.Column<int>(type: "int", nullable: false),
                    CVP = table.Column<int>(type: "int", nullable: false),
                    InsulinGiven = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GCS_Eye = table.Column<int>(type: "int", nullable: false),
                    GCS_Verbal = table.Column<int>(type: "int", nullable: false),
                    GCS_Motor = table.Column<int>(type: "int", nullable: false),
                    AdmissionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VitalSigns_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VitalSigns_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VitalSigns_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertOverrideLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverrideTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlertId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ProcedureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertOverrideLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertOverrideLogs_Alerts_AlertId",
                        column: x => x.AlertId,
                        principalTable: "Alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertOverrideLogs_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertOverrideLogs_InterventionProcedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "InterventionProcedures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Parameter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LabOrderId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabResults_LabOrders_LabOrderId",
                        column: x => x.LabOrderId,
                        principalTable: "LabOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabResults_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_RoomId",
                table: "Admissions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertOverrideLogs_AlertId",
                table: "AlertOverrideLogs",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertOverrideLogs_DoctorId",
                table: "AlertOverrideLogs",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertOverrideLogs_ProcedureId",
                table: "AlertOverrideLogs",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_AdmissionId",
                table: "Alerts",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_CurrentAdmissionId",
                table: "Beds",
                column: "CurrentAdmissionId",
                unique: true,
                filter: "[CurrentAdmissionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_RoomId",
                table: "Beds",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseHistories_AdmissionId",
                table: "CaseHistories",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseHistories_DoctorId",
                table: "CaseHistories",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_AdmissionId",
                table: "Cultures",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_DoctorId",
                table: "Cultures",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_NurseId",
                table: "Cultures",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_FluidBalances_AdmissionId",
                table: "FluidBalances",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FluidBalances_NurseId",
                table: "FluidBalances",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagings_AdmissionId",
                table: "Imagings",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagings_DoctorId",
                table: "Imagings",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionProcedures_AdmissionId",
                table: "InterventionProcedures",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionProcedures_NurseId",
                table: "InterventionProcedures",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeSources_DoctorId",
                table: "KnowledgeSources",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_AdmissionId",
                table: "LabOrders",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrders_DoctorId",
                table: "LabOrders",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_LabOrderId",
                table: "LabResults",
                column: "LabOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_NurseId",
                table: "LabResults",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_AdmissionId",
                table: "Medications",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DoctorId",
                table: "Medications",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_NursingNotes_AdmissionId",
                table: "NursingNotes",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_NursingNotes_NurseId",
                table: "NursingNotes",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalExaminations_AdmissionId",
                table: "PhysicalExaminations",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalExaminations_DoctorId",
                table: "PhysicalExaminations",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAGQueries_DoctorId",
                table: "RAGQueries",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAGQueries_PatientId",
                table: "RAGQueries",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_AdmissionId",
                table: "VitalSigns",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_DoctorId",
                table: "VitalSigns",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_NurseId",
                table: "VitalSigns",
                column: "NurseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertOverrideLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.DropTable(
                name: "CaseHistories");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "FluidBalances");

            migrationBuilder.DropTable(
                name: "Imagings");

            migrationBuilder.DropTable(
                name: "KnowledgeSources");

            migrationBuilder.DropTable(
                name: "LabResults");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "NursingNotes");

            migrationBuilder.DropTable(
                name: "PhysicalExaminations");

            migrationBuilder.DropTable(
                name: "RAGQueries");

            migrationBuilder.DropTable(
                name: "VitalSigns");

            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "InterventionProcedures");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LabOrders");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
