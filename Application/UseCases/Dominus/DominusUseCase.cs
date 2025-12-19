using Aplication.Interfaces.Dominus;
using Infraestructure.ExternalAPI.DTOs.Dominus;
using Infrastructure.ExternalAPI.Common.Response;

namespace Aplication.UseCases.Dominus
{
    public sealed class GenerarTokenDominusUseCase(IDominus dominus)
    {
        public async Task<ServiceResponse<TokenResponse>> GenerarToken(TokenParams request)
        {
            return await dominus.GenerarToken(request);
        }
    }
    
    public sealed class ConsultarListadoConsolidadosUseCase(IDominus dominus)
    {
        public async Task<ServiceResponse<ResponseListadoConsolidados>> ConsultarListadoConsolidados(RequestListadoConsolidados request)
        {
            return await dominus.ConsultarListadoConsolidados(request);
        }
    }
    public sealed class ConsultarVentasConsolidadoUseCase(IDominus dominus)
    {
        public async Task<ServiceResponse<ConsultaVentasConsolidadoResponse>> ConsultarVentasConsolidado(ConsultaVentasConsolidadoParams request)
        {
            return await dominus.ConsultarVentasConsolidado(request);
        }
    }
}