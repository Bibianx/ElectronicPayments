namespace Infraestructure.ExternalAPI.DTOs.Epayco
{
    public class facturasFiltradasDto
    {
        public Guid id_pago { get; set; }
        public string factura { get; set; }
        public string estado { get; set; }
        public decimal valor { get; set; }
    }
}