using Aplication.DTOs.Inventarios;
using Infraestructure.ExternalAPI.DTOs.Epayco;
using Infrastructure.ExternalAPI.Common.Response;

namespace Aplication.Interfaces
{
    public interface IEpayco
    {
        Task<ServiceResponse<TransactionConfirmResponse>> ConfirmacionPSE(TransactionConfirmRequest _);
        Task<ServiceResponse<TransactionResponse>> TransaccionPSE(TransactionRequest _);
        Task<ServiceResponse<INV805FResponse>> ObtenerFacturasCliente(INV805FParams _);
        Task<ServiceResponse<List<facturasFiltradasDto>>> FiltrarFacturasProcesadas();
        Task ProcesarWebHook(WebhookDto data);
        Task<ServiceResponse<string>> Login();  
    }
}