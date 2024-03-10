using System.Reflection;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Infrastructure.Database;
using UCABPagaloTodoMS.Infrastructure.Settings;
using UCABPagaloTodoMS.Providers.Implementation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RestSharp;
using MediatR;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using UCABPagaloTodoMS.Application.MappingProfiles;
using UCABPagaloTodoMS.Application.Services;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Infrastructure.Services;

namespace UCABPagaloTodoMS;

[ExcludeFromCodeCoverage]
public class Startup
{
    private AppSettings _appSettings;
    private readonly string _allowAllOriginsPolicy = "AllowAllOriginsPolicy";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        VersionNumber = "v" + Assembly.GetEntryAssembly()!
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
        Folder = "docs";
    }
    private string Folder { get; }
    private string VersionNumber { get; }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(_allowAllOriginsPolicy,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
        });
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var appSettingsSection = Configuration.GetSection("AppSettings");
        _appSettings = appSettingsSection.Get<AppSettings>();
        services.Configure<AppSettings>(appSettingsSection);
        services.AddTransient<IUCABPagaloTodoDbContext, UCABPagaloTodoDbContext>();

        services.AddProviders(Configuration, Folder, _appSettings, environment);

        //// IMPORTANTE 
       
        services.AddMediatR(typeof(ConsultarConsumidoresQueryHandler).GetTypeInfo().Assembly); // importante 
      
        ////AGREGADO NUEVO 
        
        //conexión a la BD servicios
        services.AddDbContext<UCABPagaloTodoDbContext>(options =>
        options.UseSqlServer(
        Configuration.GetConnectionString("DBConnectionString"),
        b => b.MigrationsAssembly(typeof(UCABPagaloTodoDbContext).Assembly.FullName)));
        
        // servicio del mediadot 
        services.AddMediatR(Assembly.GetExecutingAssembly());

        //interfaz de swagger
        services.AddSwaggerGen(c => 
        {
            c.IncludeXmlComments(string.Format(@"{0}\UCABPagaloTodoMS.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "UCABPagaloTodoMS Microservice",
            });
        });

        services.AddScoped<IJwtService, JwtService>(); // servicio para el token JSon 

        services.AddAutoMapper(typeof(UsuarioProfile));// servicio de mapeo de perfiles

        services.AddTransient<IMailService, MailService>(); // servicio de correo electronico

        //// 
        ///
        /// RabbitMQ
        services.AddTransient<IRabbitMQService, RabbitMQService>();
        services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
        services.AddHostedService<RabbitMqConsumerConciliacionHS>();
    }

    public void Configure(IApplicationBuilder app)
    {
        var appSettingsSection = Configuration.GetSection("AppSettings");
        _appSettings = appSettingsSection.Get<AppSettings>();

        app.UseHttpsRedirection();
        app.UseRouting();
       
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "UCABPagaloTodoMS Microservice v1");
        });
        if (_appSettings.RequireSwagger)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./" + Folder + "/swagger.json", $"UCABPagaloTodo Microservice ({VersionNumber})");
                c.InjectStylesheet(_appSettings.SwaggerStyle);
                c.DisplayRequestDuration();
                c.RoutePrefix = string.Empty;
            });
        }


        if (_appSettings.RequireAuthorization)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        if (_appSettings.RequireControllers)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready",
                    new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            });
        }
        ///SUMARY para aceptar las solicitudes HTTP desde cualquier origen
        app.UseCors(_allowAllOriginsPolicy);

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7039");
            await next.Invoke();
        });
    }
}
