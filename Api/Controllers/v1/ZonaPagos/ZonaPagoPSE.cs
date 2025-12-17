using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ZonaPagos
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]    
    [AllowAnonymous]
    public class ZonaPagosPSEController(IZonaPagoPSE ZonaPagoPSEServices) : ControllerBase
    {
        private readonly IZonaPagoPSE _servicesZonaPago = ZonaPagoPSEServices;

        [HttpPost("iniciar-pago")]
        public async Task<ActionResult<IniciarPagoResponseDto>> IniciarPago(IniciarPagoParams _)
        {
            return await _servicesZonaPago.IniciarPago(_);
        }
        [HttpPost("verificar-pago")]
        public async Task<ActionResult<VerificarPagoResponseDto>> VerificarPago(VerificarPagoParams _)
        {
            return await _servicesZonaPago.VerificarPago(_);
        }
        [HttpGet("procesar-webhook-zp")]
        public async Task ProcesarWebHook([FromQuery] int id_comercio, [FromQuery] string id_pago)
        {
            await _servicesZonaPago.ProcesarWebHook(id_comercio, id_pago);
        }
        [HttpGet("consultar-facturas")]
        public async Task<ActionResult<ServiceResponse<List<CargarFacturas>>>> CargasFacturas()
        {
            return await _servicesZonaPago.CargasFacturas();
        }
    }
}