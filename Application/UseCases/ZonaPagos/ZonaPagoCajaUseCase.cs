using Aplication.Interfaces.ZonaPagos;
using Infraestructure.ExternalAPI.DTOs.ZonaPagos;

namespace Aplication.UseCases.ZonaPagos
{
    public sealed class ConsultarFacturaUseCase(IZonaPagoCaja zonaPagoCaja)
    {
        public async Task<ConsultaPagoCajaDto> ConsultarFactura(ConsultaPagoCajaParams request)
        {
            return await zonaPagoCaja.ConsultarFactura(request);
        }
    }
    public sealed class AsentarPagoUseCase(IZonaPagoCaja zonaPagoCaja)
    {
        public async Task<AsientoPagoCajaDto> AsentarPago(AsientoPagoCajaParams request)
        {
            return await zonaPagoCaja.AsentarPago(request);
        }
    }
}