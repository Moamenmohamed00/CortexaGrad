using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Infrastructure.External;
using Cortexa.Infrastructure.Identity;
using Cortexa.Infrastructure.Persistence;
using Cortexa.Infrastructure.Persistence.Repositories;
using Cortexa.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IClinicalRepository, ClinicalRepository>();
            services.AddScoped<ILabRepository, LabRepository>();
            services.AddScoped<IImagingRepository, ImagingRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAIRepository, AIRepository>();
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
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<Application.Interfaces.Services.IIdentityService, IdentityService>();

            // ── External HTTP Clients ──────────────────────────────────
            services.AddHttpClient<AIHttpClient>(client =>
            {
                var aiBaseUrl = configuration["AIService:BaseUrl"] ?? "http://localhost:8000";
                client.BaseAddress = new Uri(aiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // ── ASP.NET Core Infrastructure ────────────────────────────
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
