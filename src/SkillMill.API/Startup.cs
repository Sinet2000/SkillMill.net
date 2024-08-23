using SkillMill.API.Configuration;
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

        // services
        //     .AddTransient<IService, Service>()
        //     ;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        AppDbSetup.Initialize(app.ApplicationServices, false);

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