global using System.ComponentModel.DataAnnotations.Schema;
global using Aplication.Services.Recaudos.ZonaPagoCaja;
global using System.ComponentModel.DataAnnotations;
global using Aplication.Services.Industria;
global using Aplication.Services.ZonaPagos;
global using Microsoft.EntityFrameworkCore;
global using FluentValidation.AspNetCore;
global using Aplication.DTOs.Industria;
global using Domain.Entities.Recaudos;
global using Polly.Extensions.Http;
global using FluentValidation;
global using AutoMapper;
global using Common;
global using Models;
global using Polly;

using Infraestructure.ExternalAPI.Mappers.ZonaPagos;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Infraestructure.ExternalAPI.DTOs.Dominus;
using Aplication.Services.Dominus;
using Aplication.Host.InicializarHost;
using Infraestructure.ExternalAPI.Common.Helpers;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        services.Configure<ClavesMonterrey>(Configuration.GetSection("CajaMonterrey"));
        services.Configure<ClavesDominus>(Configuration.GetSection("Dominus"));
        services.Configure<ClavesCAJAZP>(Configuration.GetSection("CajaZP"));
        services.Configure<ClavesPSE>(Configuration.GetSection("ZonaPagos"));


        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(Startup));

        RegisterServices(services, Configuration);
        ConfigureGeneralesMapping(services);
        ConfigureFluentValidation(services);
        ConfigureConnectionDB(services);
        ConfigureApiVersion(services);
        ConfigureCors(services);
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());
        });
    } 

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseCors(builder =>
        {
            builder
                .WithOrigins("http://localhost:8080", "http://localhost:8081")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }

    private static void ConfigureGeneralesMapping(IServiceCollection services)
        {
            /* mapping */
            services.AddAutoMapper(typeof(AutoMapperZonaPagos));
        }

    private void ConfigureConnectionDB(IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        });
    }
    
    private static void RegisterServices(IServiceCollection services, IConfiguration Configuration)
    {
        services.AddScoped<IPasarelaServices, PasarelaServices>();
        services.AddScoped<IZonaPagoCaja, ZonaPagoCajaServices>();
        services.AddScoped<IZonaPagoPSE, ZonaPagoPSEServices>();
        services.AddScoped<ISONDAServices, SONDAServices>(); 
        services.AddScoped<IIndustria, IndustriaServices>();
        services.AddScoped<IDominus, DominusServices>();

        //Host tarea en segundo plano
        services.AddHostedService<HostPagos>(); 

        //Proveedores servicios de pago externos
        ConfigureHttpClient(services, Configuration, "ZonaPagos"); //Municipio de Villanueva
        ConfigureHttpClient(services, Configuration, "Dominus"); //Estaciones de servicio
        ConfigureHttpClient(services, Configuration, "Epayco"); //Comercializadora
    }

    private static void ConfigureHttpClient(IServiceCollection services, IConfiguration configuration, string client_name)
    {

        var base_url = configuration.GetValue<string>($"ExternalServices:{client_name}:BaseUrl");

        services
            .AddHttpClient(
                client_name,
                client =>
                {
                    client.BaseAddress = new Uri(base_url);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                }
            )
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        3,
                        retryAttempt =>
                        {
                            return TimeSpan.FromSeconds(retryAttempt);
                        }
                    )
            );
    }

    private static void ConfigureFluentValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Startup>();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
    }
    
    private static void ConfigureApiVersion(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}
