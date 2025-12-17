using Infraestructure.ExternalAPI.DTOs.Dominus;
using Aplication.Services.Dominus;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Dominus
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DominusController(IDominus dominus_service) : ControllerBase
    {
        private readonly IDominus _dominusService = dominus_service;

        [HttpPost("autenticacion/token")]
        public async Task<ActionResult<ServiceResponse<TokenResponse>>> GenerarToken([FromBody] TokenParams request)
        {
            return await _dominusService.GenerarToken(request);
        }

        [HttpGet("consolidated/list")]
        public async Task<ActionResult<ServiceResponse<ResponseListadoConsolidados>>> ConsultarListadoConsolidados([FromQuery] RequestListadoConsolidados request)
        {
            return await _dominusService.ConsultarListadoConsolidados(request);
        }

        [HttpGet("consolidated/sales")]
        public async Task<ActionResult<ServiceResponse<ConsultaVentasConsolidadoResponse>>> ConsultarVentasConsolidado([FromQuery] ConsultaVentasConsolidadoParams request)
        {
            return await _dominusService.ConsultarVentasConsolidado(request);
        }
    }   
}