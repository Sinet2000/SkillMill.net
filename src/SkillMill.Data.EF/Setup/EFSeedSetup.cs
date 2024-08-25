using Microsoft.Extensions.DependencyInjection;
using SkillMill.Data.Common.Fakers;

namespace SkillMill.Data.EF;

public class EFSeedSetup
{
    public static void Configure(IServiceCollection services)
    {
        AddFakers(services);

        AddSeeds(services);
    }
    
    private static void AddFakers(IServiceCollection services)
    {
        services
            .AddScoped<CustomerFaker>()
            .AddScoped<OrderFaker>()
            .AddScoped<OrderItemFaker>()
            .AddScoped<ProductFaker>();
    }

    private static void AddSeeds(IServiceCollection services)
    {
        services
            .AddScoped<CustomerEfDataSeed>();
    }
}