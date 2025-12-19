using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Infrastructure.ExternalAPI.Common.Response;
using Aplication.Interfaces.ZonaPagos;

namespace Aplication.UseCases.ZonaPagos
{
    public sealed class IniciarPagoUseCase(IZonaPagoPSE zonaPagoPSE)
    {
        public async Task<InicioPagoResponsePSEDto> ConsultarFactura(InicioPagoPSEParams request)
        {
            return await zonaPagoPSE.IniciarPago(request);
        }
    }
    public sealed class VerificarPagoUseCase(IZonaPagoPSE zonaPagoPSE)
    {
        public async Task<VerificacionPagoPSEResponse> VerificarPago(VerificacionPagoPSEParams request)
        {
            return await zonaPagoPSE.VerificarPago(request);
        }
    }
    
    public sealed class CargasFacturasUseCase(IZonaPagoPSE zonaPagoPSE)
    {
        public async Task<ServiceResponse<List<FacturaParams>>> CargasFacturas()
        {
            return await zonaPagoPSE.CargasFacturas();
        }
    }

    public sealed class ProcesarWebHookUseCase(IZonaPagoPSE zonaPagoPSE)
    {
        public async Task ProcesarWebHook(int id_comercio, string id_pago)
        {
            await zonaPagoPSE.ProcesarWebHook(id_comercio, id_pago);
        }
    }

}