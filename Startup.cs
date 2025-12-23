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
using Infraestructure.ExternalAPI.Services.Dominus;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Infraestructure.ExternalAPI.Common.Helpers;
using Infraestructure.ExternalAPI.DTOs.Dominus;
using Infraestructure.ExternalAPI.DTOs.Epayco;
using Aplication.Interfaces.ZonaPagos;
using Aplication.Host.InicializarHost;
using Aplication.UseCases.ZonaPagos;
using Aplication.Interfaces.Dominus;
using Aplication.UseCases.Dominus;
using Infrastructure.ExternalAPI.Services.Epayco;
using Aplication.Interfaces;
using Aplication.UseCases;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        services.Configure<ClavesMonterrey>(Configuration.GetSection("CajaMonterrey"));
        services.Configure<EpaycoCredentials>(Configuration.GetSection("Epayco"));
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
        RegisterUseCases(services);
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

        app.UseCors("AllowAllOrigins");
        
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
        //Servicios infraestructura comun 
        services.AddScoped<IPasarelaServices, PasarelaServices>();
        services.AddScoped<IZonaPagoCaja, ZonaPagoCajaServices>();
        services.AddScoped<IZonaPagoPSE, ZonaPagoPSEServices>();
        services.AddScoped<ISONDAServices, SONDAServices>(); 
        services.AddScoped<IIndustria, IndustriaServices>();
        services.AddScoped<IDominus, DominusServices>();
        services.AddScoped<IEpayco, EpaycoServices>();
        
        //Host tarea en segundo plano
        services.AddHostedService<HostPagos>(); 

        //Proveedores servicios de pago externos
        ConfigureHttpClient(services, Configuration, "ZonaPagos"); //Municipio de Villanueva
        ConfigureHttpClient(services, Configuration, "Dominus"); //Estaciones de servicio
        ConfigureHttpClient(services, Configuration, "Epayco"); //Comercializadora
    }

    private static void RegisterUseCases(IServiceCollection services)
    {
        //Casos de uso Dominus
        services.AddScoped<ConsultarListadoConsolidadosUseCase>();
        services.AddScoped<ConsultarVentasConsolidadoUseCase>();
        services.AddScoped<GenerarTokenDominusUseCase>();

        //Casos de uso ZonaPagos Caja
        services.AddScoped<ConsultarFacturaUseCase>();
        services.AddScoped<AsentarPagoUseCase>();

        //Casos de uso ZonaPagos PSE
        services.AddScoped<ProcesarWebHookUseCase>();
        services.AddScoped<CargasFacturasUseCase>();
        services.AddScoped<VerificarPagoUseCase>();
        services.AddScoped<IniciarPagoUseCase>();

        //Casos de uso Epayco
        services.AddScoped<FiltrarFacturasClienteUseCase>();
        services.AddScoped<ObtenerFacturasClienteUseCase>();
        services.AddScoped<TransaccionPSEUseCase>();
        services.AddScoped<ConfirmarPSEUseCase>();
        services.AddScoped<BasicAuthUseCase>();
        services.AddScoped<WebHookUseCase>();
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
