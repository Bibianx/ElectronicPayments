namespace Aplication.DTOs.Industria
{
    public class RequestIYC003 : RequestIYC003R;

    public class ResponseIYC003
    {
        public string STATUS { get; set; }
        public MensajeIYC003 MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003
    {
        public List<DatoIYC003> datos { get; set; }
    }

    public class DatoIYC003
    {
        public string nit { get; set; }
        public string periodo { get; set; }
        public string ficha { get; set; }
        public string fecha_creacion { get; set; }
        public string saldo_favor { get; set; }
        public string estado { get; set; }
        public string tipo_form { get; set; }
        public string reg_iva { get; set; }
        public string modific { get; set; }
    }

}