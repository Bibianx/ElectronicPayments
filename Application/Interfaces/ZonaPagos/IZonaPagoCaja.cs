using Infraestructure.ExternalAPI.DTOs.ZonaPagos;

namespace Aplication.Interfaces.ZonaPagos
{
    public interface IZonaPagoCaja
    {
        Task<ConsultaPagoCajaDto> ConsultarFactura(ConsultaPagoCajaParams _);
        Task<AsientoPagoCajaDto> AsentarPago(AsientoPagoCajaParams _);
    }
}