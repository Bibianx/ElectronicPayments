using Infrastructure.ExternalAPI.Common.Response;
using Infraestructure.ExternalAPI.DTOs.Dominus;
using Aplication.UseCases.Dominus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Dominus
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DominusController(
        ConsultarListadoConsolidadosUseCase listarConsolidados, 
        ConsultarVentasConsolidadoUseCase ventasConsolidado,
        GenerarTokenDominusUseCase generarToken
    ) : ControllerBase
    {

        [HttpGet("consolidated/list")]
        public async Task<ServiceResponse<ResponseListadoConsolidados>> ConsultarListadoConsolidados([FromQuery] RequestListadoConsolidados request)
        {
            return await listarConsolidados.ConsultarListadoConsolidados(request);
        } 

        [HttpGet("consolidated/sales")]
        public async Task<ServiceResponse<ConsultaVentasConsolidadoResponse>> ConsultarVentasConsolidado([FromQuery] ConsultaVentasConsolidadoParams request)
        {
            return await ventasConsolidado.ConsultarVentasConsolidado(request);
        }
        
        [HttpPost("autenticacion/token")]
        public async Task<ServiceResponse<TokenResponse>> GenerarToken([FromBody] TokenParams request)
        {
            return await generarToken.GenerarToken(request);
        }
    }
}