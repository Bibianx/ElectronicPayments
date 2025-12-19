using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Microsoft.AspNetCore.Authorization;
using Aplication.UseCases.ZonaPagos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ZonaPagos
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]    
    [AllowAnonymous]
    public class ZonaPagosCajaController(
        ConsultarFacturaUseCase consultarFacturaUseCase,
        AsentarPagoUseCase asentarPagoUseCase
    ) : ControllerBase
    {
        [HttpPost("consultar-pago")]
        public async Task<ActionResult<ConsultaPagoCajaDto>> ConsultarFacturas(ConsultaPagoCajaParams _)
        {
            return await consultarFacturaUseCase.ConsultarFactura(_);
        }

        [HttpPost("asentar-pago")]
        public async Task<ActionResult<AsientoPagoCajaDto>> AsentarPago(AsientoPagoCajaParams _)
        {
            return await asentarPagoUseCase.AsentarPago(_);
        }
        
    }
}