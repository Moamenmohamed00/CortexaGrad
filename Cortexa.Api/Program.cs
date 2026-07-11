using Cortexa.Application;
using Cortexa.Infrastructure;
using Cortexa.Infrastructure.Persistence.Seeding;
using Cortexa.Api.Extensions;
using Cortexa.Api.Hubs;
using Cortexa.Api.Middlewares;
using Scalar.AspNetCore;

// Load environment variables from .env file
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
