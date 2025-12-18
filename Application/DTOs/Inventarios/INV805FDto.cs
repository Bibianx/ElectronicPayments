namespace Aplication.DTOs.Inventarios
{
     public class INV805FResponse
    {
        public string STATUS { get; set; }
        public Mensaje MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class Mensaje
    {
        public TercerosData TERCEROS { get; set; }
        public List<FacturasData> FACTURAS { get; set; }
    }

    public class TercerosData
    {
        public string tipo_id { get; set; }
        public string id { get; set; }
        public string nombres { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
    }

    public class FacturasData
    {
        public string sucursal { get; set; }
        public string factura { get; set; }
        public string item { get; set; }
        public string nro_cuota { get; set; }
        public string fecha_vence { get; set; }
        public string saldo_actual { get; set; }
        public string monto_inicial { get; set; }
        public string forma_pago { get; set; }
        public string vendedor { get; set; }
    }
}