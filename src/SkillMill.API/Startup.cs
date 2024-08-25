using System.Reflection;
using Sieve.Models;
using Sieve.Services;
using SkillMill.API.Configuration;
using SkillMill.Application;
using SkillMill.Application.Problems;
using SkillMill.Data.Common;
using SkillMill.Data.Common.Models;
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
        services.AddScoped<SieveProcessor>();
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();

        var coreDbSetup = new AppDbSetup(configuration);
        coreDbSetup.Configure(services);

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

        services.AddAutoMapper(
            typeof(CartesianExplosionProblem).Assembly,
            typeof(DataConst).Assembly);

        // services
        //     .AddTransient<ITestService, TestService>()
        //     ;
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
}