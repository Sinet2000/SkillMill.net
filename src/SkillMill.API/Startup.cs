using System.Reflection;
using Mapster;
using MapsterMapper;
using Sieve.Models;
using Sieve.Services;
using SkillMill.API.Configuration;
using SkillMill.Application;
using SkillMill.Application.Customers;
using SkillMill.Data.Common.Interfaces;
using SkillMill.Data.EF;
using SkillMill.Data.EF.Interfaces;
using SkillMill.Data.EF.Setup;

namespace SkillMill.API;

public class Startup(IConfiguration configuration)
{

    public void ConfigureServices(IServiceCollection services)
    {
        // Default logger
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
        });

        services.Configure<CoreAppConfiguration>(configuration.GetSection(CoreAppConfiguration.SectionName));
        var appConfiguration = configuration.GetSection(CoreAppConfiguration.SectionName).Get<CoreAppConfiguration>();
        ArgumentNullException.ThrowIfNull(appConfiguration);

        // https://github.com/Biarity/Sieve?tab=readme-ov-file
        services.Configure<SieveOptions>(configuration.GetSection("Sieve"));
        services.AddScoped<ISieveProcessor, SieveProcessor>();
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();

        var coreDbSetup = new AppDbSetup(configuration);
        coreDbSetup.Configure(services);

        services.AddScoped<IDataSearchQuery<AppDbContext>, DataSearchableDbContext<AppDbContext>>();
        services.AddScoped<IUpdateableDbContext<AppDbContext>, UpdateableDbContext<AppDbContext>>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin();
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                });
        });

        services.AddSwaggerGen();

        services.AddControllers();

        ConfigureMapster(services);

        services
            .AddTransient<ICustomerService, CustomerService>()
            ;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        AppDbSetup.Initialize(app.ApplicationServices, ensureDeleted: false);

        app.UseCors();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (!env.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private static void ConfigureMapster(IServiceCollection services)
    {
        var mapsterConfigTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IMapsterConfig).IsAssignableFrom(t) && t.IsClass);

        foreach (var configType in mapsterConfigTypes)
        {
            var configInstance = Activator.CreateInstance(configType) as IMapsterConfig;
            configInstance?.Configure();
        }

        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}