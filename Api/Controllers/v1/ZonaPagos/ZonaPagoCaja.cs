using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ZonaPagos
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]    
    [AllowAnonymous]
    public class ZonaPagosCajaController(IZonaPagoCaja ZonaPagoCajaServices) : ControllerBase
    {
        private readonly IZonaPagoCaja _servicesZonaPagoCaja = ZonaPagoCajaServices;

        [HttpPost("consultar-pago")]
        public async Task<ActionResult<ConsultaPagoDto>> ConsultarFacturas(ConsultaPagoParams _)
        {
            return await _servicesZonaPagoCaja.ConsultarFactura(_);
        }

        [HttpPost("asentar-pago")]
        public async Task<ActionResult<AsientoPagoDto>> AsentarPago(AsientoPagoParams _)
        {
            return await _servicesZonaPagoCaja.AsentarPago(_);
        }
        
    }
}