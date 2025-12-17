namespace Infraestructure.ExternalAPI.DTOs.ZonaPagos
{
    public class FacturaParams
    {
        public Guid id_pago { get; set; }
        public decimal flt_total_con_iva { get; set; }
        public decimal flt_valor_iva { get; set; }
        public string str_id_pago { get; set; }
        public string str_descripcion_pago { get; set; }
        public string str_id_cliente { get; set; }
        public string estado_intento { get; set; }
        public DateOnly? fecha_intento { get; set; }
        public TimeOnly? hora_intento { get; set; }
    }
}