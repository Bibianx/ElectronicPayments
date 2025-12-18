namespace Aplication.DTOs.Industria
{
    public class RequestIYC003R
    {
        public string sesion { get; set; }
        public string usuario { get; set; }
        public int tipo_form { get; set; } // 1: declaracion bimestral, 2: declaracion anual
    }

    public class ResponseIYC003R
    {
        public string STATUS { get; set; }
        public MensajeIYC003R MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003R
    {
        public List<DatoIYC003R> datos { get; set; }
    }

    public class DatoIYC003R
    {
        public string nit { get; set; }
        public string periodo { get; set; }
        public string ficha { get; set; }
        public string fecha_creacion { get; set; }
        public string estado { get; set; }
        public string abonos { get; set; }
        public string total_fact { get; set; }
        public string ticket { get; set; }
        public string saldo_fact { get; set; }
    }
}