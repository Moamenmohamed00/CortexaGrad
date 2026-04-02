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

builder.Services.AddSwaggerGen();


var app = builder.Build();

// ── Database Seeding (Development) ─────────────────────────────────
if (app.Environment.IsDevelopment())
{
    await DatabaseSeeder.SeedAsync(app.Services);
}

// ── HTTP Request Pipeline ──────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<ApiKeyMiddleware>();


app.MapControllers();

// ── SignalR Hubs ───────────────────────────────────────────────────
app.MapHub<AlertHub>("/hubs/alerts");
app.MapHub<MonitoringHub>("/hubs/monitoring");
//app.MapGet("/", () => Results.Ok(new
//{
//    service = "Cortexa API",
//    status = "Running",
//    environment = app.Environment.EnvironmentName,
//    time = DateTime.UtcNow
//})); 
app.Run();
//after finish use code wiki to make readme file