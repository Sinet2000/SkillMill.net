using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkillMill.Data.Common;

namespace SkillMill.Data.EF.Setup;

public class AppDbSetup(IConfiguration configuration)
{
    public void Configure(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            options =>
                options.UseSqlServer(
                        configuration["CoreAppConfig:DbConnectionString"],
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(10);
                            sqlOptions.CommandTimeout(300);
                            sqlOptions.MinBatchSize(1);
                            sqlOptions.MaxBatchSize(100);
                        })
                    .EnableSensitiveDataLogging(false)
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information),
            ServiceLifetime.Transient);

        services.AddScoped<SeedApplier>();

        EFSeedSetup.Configure(services);
    }

    public static void Initialize(IServiceProvider hostServices, bool ensureDeleted = false)
    {
        Task.Run(async () =>
        {
            using (var scope = hostServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();

                if (ensureDeleted)
                {
                    await dbContext.Database.EnsureDeletedAsync();
                }

                await dbContext.Database.MigrateAsync();

                var seedApplier = services.GetRequiredService<SeedApplier>();
                await seedApplier.ApplySeedAsync<CustomerEfDataSeed>();
            }
        }).GetAwaiter().GetResult();
    }
}