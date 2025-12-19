using Aplication.UseCases.ZonaPagos;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Infrastructure.ExternalAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ZonaPagos
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]    
    [AllowAnonymous]
    public class ZonaPagosPSEController(
        ProcesarWebHookUseCase procesarWebHookUseCase,
        CargasFacturasUseCase cargasFacturasUseCase,
        VerificarPagoUseCase verificarPagoUseCase,
        IniciarPagoUseCase iniciarPagoUseCase
    ) : ControllerBase
    {
        [HttpPost("iniciar-pago")]
        public async Task<ActionResult<InicioPagoResponsePSEDto>> ConsultarFacturas(InicioPagoPSEParams _)
        {
            return await iniciarPagoUseCase.ConsultarFactura(_);
        }

        [HttpPost("verificar-pago")]
        public async Task<ActionResult<VerificacionPagoPSEResponse>> VerificarPago(VerificacionPagoPSEParams _)
        {
            return await verificarPagoUseCase.VerificarPago(_);
        }

        [HttpGet("consultar-facturas")]
        public async Task<ActionResult<ServiceResponse<List<FacturaParams>>>> CargasFacturas()
        {
            return await cargasFacturasUseCase.CargasFacturas();
        }

        [HttpPost("procesar-webhook-zp")]
        public async Task<IActionResult> ProcesarWebHook([FromQuery] int id_comercio, [FromQuery] string id_pago)
        {
            await procesarWebHookUseCase.ProcesarWebHook(id_comercio, id_pago);
            return Ok();
        }
    }
}