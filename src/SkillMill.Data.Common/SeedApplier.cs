using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SkillMill.Data.Common;

/// <summary>
/// Applies seed data using services provided by IServiceProvider.
/// </summary>
public class SeedApplier(IServiceProvider services, ILogger<SeedApplier> logger)
{
    public async Task ApplySeedAsync<TApplySeedAsync>()
        where TApplySeedAsync : ISeedApplier
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var seed = services.GetRequiredService<TApplySeedAsync>();
            logger.LogInformation("Starting to apply seed using {SeedType}.", typeof(TApplySeedAsync).Name);

            await seed.ApplyAsync();

            stopwatch.Stop();
            logger.LogInformation("Seed applied successfully using {SeedType}. Time taken: {ElapsedSeconds} seconds.",
                typeof(TApplySeedAsync).Name, stopwatch.Elapsed.TotalSeconds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying seed using {SeedType}.", typeof(TApplySeedAsync).Name);

            throw;
        }
    }
}