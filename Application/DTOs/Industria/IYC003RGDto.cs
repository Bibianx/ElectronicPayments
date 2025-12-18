namespace Aplication.DTOs.Industria
{
    public class IYC003RGRequest
    {
        public string sesion { get; set; }
        public string usuario { get; set; }
        public string llave { get; set; }
        public decimal imp_pago { get; set; }
        public decimal avi_pago { get; set; }
        public decimal bom_pago { get; set; }
        public decimal otro_pago { get; set; }
        public decimal impuestos { get; set; }
        public decimal sancion { get; set; }
        public decimal interes { get; set; }
    }
}