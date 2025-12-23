using Aplication.DTOs.Inventarios;
using Aplication.Interfaces;
using Infraestructure.ExternalAPI.DTOs.Epayco;
using Infrastructure.ExternalAPI.Common.Response;

namespace Aplication.UseCases
{
    public sealed class BasicAuthUseCase(IEpayco epayco)
    {
        public async Task<ServiceResponse<string>> Login()
        {
            return await epayco.Login();
        }
    }
    
    public sealed class TransaccionPSEUseCase(IEpayco epayco)
    {
        public async Task<ServiceResponse<TransactionResponse>> TransaccionPSE(TransactionRequest _request)
        {
            return await epayco.TransaccionPSE(_request);
        }
    }
    
    public sealed class ObtenerFacturasClienteUseCase(IEpayco epayco)
    {
        public async Task<ServiceResponse<INV805FResponse>> ObtenerFacturasCliente(INV805FParams _request)
        {
            return await epayco.ObtenerFacturasCliente(_request);
        }
    }
    public sealed class FiltrarFacturasClienteUseCase(IEpayco epayco)
    {
        public async Task<ServiceResponse<List<facturasFiltradasDto>>> FiltrarFacturasProcesadas()
        {
            return await epayco.FiltrarFacturasProcesadas();
        }
    }
    
    public sealed class ConfirmarPSEUseCase(IEpayco epayco)
    {
        public async Task<ServiceResponse<TransactionConfirmResponse>> ConfirmacionPSE(TransactionConfirmRequest _)
        {
            return await epayco.ConfirmacionPSE(_); 
        }
    }
    
    public sealed class WebHookUseCase(IEpayco epayco)
    {
        public async Task ProcesarWebHook(WebhookDto data)
        {
            await epayco.ProcesarWebHook(data);
        }
    }
}