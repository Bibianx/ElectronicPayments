using Infraestructure.ExternalAPI.Common.Helpers;

namespace Aplication.Host.InicializarHost
{
    public class HostPagos(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider scopeFactory = serviceProvider;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = scopeFactory.CreateScope();
                var sonda = scope.ServiceProvider.GetRequiredService<ISONDAServices>();

                await sonda.ConsultarPagosPendientes(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }

}