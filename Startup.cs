using Aplication.DTOs.ZonaPagos;
using Aplication.Services.Industria;
using Aplication.Services.ZonaPagos;
using Aplication.Services.Recaudos.ZonaPagoCaja;
using Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infraestructure.BackgroundServices;
using Infraestructure.Mappers;
using Microsoft.EntityFrameworkCore;
using Models;
using Polly;
using Polly.Extensions.Http;

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
