using SkillMill.API.Configuration;
using SkillMill.Data.EF.Setup;

namespace SkillMill.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var webAppBuilder = WebApplication.CreateBuilder(args);

        webAppBuilder.ConfigureServices();

        var app = webAppBuilder.Build();
        await app.Configure(webAppBuilder);
        await app.RunAsync();
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;

        services.Configure<CoreAppConfiguration>(config.GetSection(CoreAppConfiguration.SectionName));
        var appConfiguration = config.GetSection(CoreAppConfiguration.SectionName).Get<CoreAppConfiguration>();
        ArgumentNullException.ThrowIfNull(appConfiguration);

        var coreDbSetup = new AppDbSetup(appConfiguration.DbConnectionString);
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
    }

    private static async Task Configure(this IApplicationBuilder app, WebApplicationBuilder builder)
    {
        var env = builder.Environment;

        await AppDbSetup.Initialize(app.ApplicationServices);

        app.UseCors();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.ConfigureSwagger(env);
    }

    private static void ConfigureSwagger(this IApplicationBuilder app, IHostEnvironment env)
    {
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
    }
}