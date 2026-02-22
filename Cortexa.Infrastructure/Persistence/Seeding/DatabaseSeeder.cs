using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.Persistence.Seeding
{
    /// <summary>
    /// Orchestrates all database seeders in the correct order.
    /// Call from Program.cs during application startup.
    /// </summary>
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<CortexaDbContext>>();

            try
            {
                var context = services.GetRequiredService<CortexaDbContext>();

                // Recreate database to pick up schema changes (Identity tables, etc.)
                // WARNING: This deletes all data — suitable for development only.
                //await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                // 1. Master data first (Rooms & Beds)
                var masterSeeder = new MasterDataSeeder(
                    context,
                    services.GetRequiredService<ILogger<MasterDataSeeder>>());
                await masterSeeder.SeedAsync();

                // 2. Default users (Doctors, Nurses, Patients)
                var usersSeeder = new DefaultUsersSeeder(
                    context,
                    services.GetRequiredService<ILogger<DefaultUsersSeeder>>());
                await usersSeeder.SeedAsync();

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
