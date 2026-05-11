using System.Text;
using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Infrastructure.External;
using Cortexa.Infrastructure.Identity;
using Cortexa.Infrastructure.Persistence;
using Cortexa.Infrastructure.Persistence.Repositories;
using Cortexa.Infrastructure.Persistence.Repositories.Clinical;
using Cortexa.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Cortexa.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── Database ───────────────────────────────────────────────
            services.AddDbContext<CortexaDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(CortexaDbContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                    }));

            services.AddScoped<IApplicationDbContext>(sp =>
                sp.GetRequiredService<CortexaDbContext>());

            // ── ASP.NET Identity ────────────────────────────────────────
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password policy
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Lockout
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // User
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<CortexaDbContext>()
            .AddDefaultTokenProviders();

            // ── Repositories ───────────────────────────────────────────
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAdmissionRepository, AdmissionRepository>();
            //services.AddScoped<IClinicalRepository, ClinicalRepository>();
            services.AddScoped<IVitalSignsRepository, VitalSignsRepository>();
            services.AddScoped<IMedicationRepository, MedicationsRepository>();
            services.AddScoped<INursingNotesRepository, NursingNotesRepository>();
            services.AddScoped<IFluidBalanceRepository, FluidBalanceRepository>();
            services.AddScoped<ICaseHistoryRepository, CaseHistoryRepository>();
            services.AddScoped<IPhysicalExaminationRepository, PhysicalExaminationRepository>();
            services.AddScoped<IInterventionProcedureRepository, InterventionProcedureRepository>();
            services.AddScoped<ILabRepository, LabRepository>();
            services.AddScoped<IImagingRepository, ImagingRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<INurseRepository, NurseRepository>();
            services.AddScoped<IBedRepository, BedRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();  
            services.AddScoped<IAIRepository, AIRepository>();
            services.AddScoped<IRagRepository, RagRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Services ───────────────────────────────────────────────
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<EmailSettings>(
                configuration.GetSection(EmailSettings.SectionName));
            services.AddTransient<IEmailService, EmailService>();
            // INotificationService is registered in the Api layer (SignalRNotificationService)

            // ── Identity & JWT ─────────────────────────────────────────
            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // ── JWT Bearer Authentication ──────────────────────────────
            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings configuration is missing.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey  = true,
                    ValidIssuer              = jwtSettings.Issuer,
                    ValidAudience            = jwtSettings.Audience,
                    IssuerSigningKey         = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    // Respect ClaimTypes.Role used by JwtTokenGenerator
                    RoleClaimType            = System.Security.Claims.ClaimTypes.Role
                };
            });

            // IIdentityService is registered in both Application and Common interfaces to allow for flexibility in referencing it from different layers without causing circular dependencies.
            //لو حصل مشكله هتبقى بسبب الموضوع ده
            services.AddScoped<IAIService, PythonRAGService>();
            services.AddScoped<Application.Common.Interfaces.IIdentityService, IdentityService>();
            services.AddScoped<Application.Interfaces.Services.IIdentityService, IdentityService>();

            // ── External HTTP Clients ──────────────────────────────────
            services.AddHttpClient<AIHttpClient>(client =>
            {
                var aiBaseUrl = configuration["AIService:BaseUrl"]
                    ?? "https://m0amenmohamed-rag.hf.space";
                client.BaseAddress = new Uri(aiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(60); // HF Space may cold-start
            });

            // ── ASP.NET Core Infrastructure ────────────────────────────
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
