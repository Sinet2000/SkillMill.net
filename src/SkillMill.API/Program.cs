using SkillMill.API.Configuration;

namespace SkillMill.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var webAppBuilder = WebApplication.CreateBuilder(args);
        
        webAppBuilder.ConfigureServices();
        
        var app = webAppBuilder.Build();
        app.Configure(webAppBuilder);
        await app.RunAsync();
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;

        services.Configure<CoreAppConfiguration>(config.GetSection(CoreAppConfiguration.SectionName));

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

    private static void Configure(this IApplicationBuilder app, WebApplicationBuilder builder)
    {
        var env = builder.Environment;
        app.UseCors();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

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