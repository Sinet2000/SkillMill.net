using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkillMill.Data.Common;

namespace SkillMill.Data.EF.Setup;

public class AppDbSetup(string dbConnectionString)
{
    public void Configure(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            options =>
                options.UseSqlServer(
                    dbConnectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(10);
                        sqlOptions.CommandTimeout(300);
                        sqlOptions.MinBatchSize(1);
                        sqlOptions.MaxBatchSize(100);
                    }).EnableSensitiveDataLogging(false),
            ServiceLifetime.Transient);

        services.AddScoped<SeedApplier>();

        EFSeedSetup.Configure(services);
    }

    public static async Task Initialize(IServiceProvider hostServices, bool ensureDeleted = false)
    {
        using (var scope = hostServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();

            if (ensureDeleted)
            {
                await dbContext.Database.EnsureDeletedAsync();
            }

            await dbContext.Database.EnsureCreatedAsync();

            var seedApplier = services.GetRequiredService<SeedApplier>();
            await seedApplier.ApplySeedAsync<CustomerEFDataSeed>();
        }
    }
}