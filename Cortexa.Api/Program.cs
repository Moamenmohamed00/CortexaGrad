using Cortexa.Application;
using Cortexa.Infrastructure;
using Cortexa.Infrastructure.Persistence.Seeding;
using Cortexa.Api.Extensions;
using Cortexa.Api.Hubs;
using Cortexa.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ── Service Registration ───────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();

// ── CORS ───────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ── Database Seeding (Development) ─────────────────────────────────
if (app.Environment.IsDevelopment())
{
    await DatabaseSeeder.SeedAsync(app.Services);
}

// ── HTTP Request Pipeline ──────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

// ── SignalR Hubs ───────────────────────────────────────────────────
app.MapHub<AlertHub>("/hubs/alerts");
app.MapHub<MonitoringHub>("/hubs/monitoring");

app.Run();
//after finish use code wiki to make readme file