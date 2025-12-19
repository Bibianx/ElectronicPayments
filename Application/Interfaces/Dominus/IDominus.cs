using Infraestructure.ExternalAPI.DTOs.Dominus;
using Infrastructure.ExternalAPI.Common.Response;

namespace Aplication.Interfaces.Dominus
{
    public interface IDominus
    {
        Task<ServiceResponse<TokenResponse>> GenerarToken(TokenParams request);
        Task<ServiceResponse<ResponseListadoConsolidados>> ConsultarListadoConsolidados(RequestListadoConsolidados request);
        Task<ServiceResponse<ConsultaVentasConsolidadoResponse>> ConsultarVentasConsolidado(ConsultaVentasConsolidadoParams request);
    }
}