using Cortexa.Api.Filters;
using Cortexa.Api.Hubs;
using Cortexa.Api.Services;
using Cortexa.Application.Interfaces.Services;
using Microsoft.OpenApi;

namespace Cortexa.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // ── Controllers ────────────────────────────────────────────
            services.AddControllers(options =>
            {
                options.Filters.Add<PerformanceLoggingFilter>();
            });
            services.AddEndpointsApiExplorer();

            // ── Swagger ────────────────────────────────────────────────
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cortexa API",
                    Version = "v1",
                    Description = "Cortexa Hospital Management System API"
                });

                // JWT Bearer security definition
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter your JWT token: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // ApiKey security definition
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "X-Api-Key",
                    Description = "API Key authentication via header",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // Global security requirement
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>()
                    },
                    {
                        new OpenApiSecuritySchemeReference("ApiKey", document),
                        new List<string>()
                    }
                });
            });

            // ── SignalR ────────────────────────────────────────────────
            services.AddSignalR();

            // Register SignalR-backed NotificationService
            services.AddScoped<INotificationService, SignalRNotificationService>();

            return services;
        }
    }
}
