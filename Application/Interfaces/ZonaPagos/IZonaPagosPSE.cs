using Infraestructure.ExternalAPI.DTOs.ZonaPagos;
using Infrastructure.ExternalAPI.Common.Response;

namespace Aplication.Interfaces.ZonaPagos
{
    public interface IZonaPagoPSE
    {
        Task<VerificacionPagoPSEResponse> VerificarPago(VerificacionPagoPSEParams _);
        Task<InicioPagoResponsePSEDto> IniciarPago(InicioPagoPSEParams _);
        Task<ServiceResponse<List<FacturaParams>>> CargasFacturas();
        Task ProcesarWebHook(int id_comercio, string id_pago);
    }

}