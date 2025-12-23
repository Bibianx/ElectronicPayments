using Aplication.DTOs.Inventarios;
using Aplication.UseCases;
using Infraestructure.ExternalAPI.DTOs.Epayco;
using Infrastructure.ExternalAPI.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Epayco
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EpaycoController(
        FiltrarFacturasClienteUseCase filtrarFacturasClienteUseCase,
        ObtenerFacturasClienteUseCase obtenerFacturasClienteUseCase,
        TransaccionPSEUseCase TransaccionPSEUseCase,
        ConfirmarPSEUseCase confirmarPSEUseCase,
        BasicAuthUseCase basicAuthUseCase,
        WebHookUseCase webHookUseCase
    ) : ControllerBase
    {
        [HttpGet("confirmar/pse")]
        public async Task<ActionResult<ServiceResponse<TransactionConfirmResponse>>> ConfirmacionPSE(TransactionConfirmRequest _)
        {
            return await confirmarPSEUseCase.ConfirmacionPSE(_);
        }

        [HttpPost("transaccion/pse")]
        public async Task<ActionResult<ServiceResponse<TransactionResponse>>> TransaccionPSE(TransactionRequest _)
        {
            return await TransaccionPSEUseCase.TransaccionPSE(_);
        }

        [HttpPost("facturas/cliente")]
        public async Task<ActionResult<ServiceResponse<INV805FResponse>>> ObtenerFacturasCliente(INV805FParams _)
        {
            return await obtenerFacturasClienteUseCase.ObtenerFacturasCliente(_);
        }


        [HttpGet("facturas/filtrar-procesadas")]
        public async Task<ActionResult<ServiceResponse<List<facturasFiltradasDto>>>> FiltrarFacturasProcesadas()
        {
            return await filtrarFacturasClienteUseCase.FiltrarFacturasProcesadas();
        }

        [HttpPost("webhook/procesar")]
        public async Task<IActionResult> ProcesarWebHook(WebhookDto data)
        {
            await webHookUseCase.ProcesarWebHook(data);
            return Ok();
        }

        [HttpPost("autenticacion/login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login()
        {
            return await basicAuthUseCase.Login();
        }
    }
}
