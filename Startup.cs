global using Aplication.Services.Recaudos.ZonaPagoCaja;
global using Infraestructure.BackgroundServices;
global using Aplication.Services.Industria;
global using Aplication.Services.ZonaPagos;
global using Microsoft.EntityFrameworkCore;
global using FluentValidation.AspNetCore;
global using Aplication.DTOs.ZonaPagos;
global using Aplication.DTOs.Industria;
global using Domain.Entities.Recaudos;
global using Infraestructure.Mappers;
global using Polly.Extensions.Http;
global using FluentValidation;
global using Models.Response;
global using AutoMapper;
global using Common;
global using Models;
global using Polly;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        services.Configure<ClavesMonterrey>(Configuration.GetSection("CajaMonterrey"));
        services.Configure<ClavesCAJAZP>(Configuration.GetSection("CajaZP"));
        services.Configure<ClavesPSE>(Configuration.GetSection("ZonaPagos"));


        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null; // opcional
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(Startup));

        ConfigureGeneralesMapping(services);
        ConfigureFluentValidation(services);
        ConfigureConnectionDB(services);
        ConfigureApiVersion(services);
        RegisterServices(services);
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
    
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IPasarelaServices, PasarelaServices>();
        services.AddScoped<IZonaPagoPSE, ZonaPagoPSEServices>();
         services
                .AddHttpClient(
                    "ZonaPagos",
                    client =>
                    {
                        client.BaseAddress = new Uri("https://www.zonapagos.com/");
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
        services.AddScoped<IZonaPagoCaja, ZonaPagoCajaServices>();
        services.AddScoped<IIndustria, IndustriaServices>();
        services.AddHostedService<SONDAServices>();
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
