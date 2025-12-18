namespace Aplication.DTOs.Industria
{
    public class RequestIYC003RI : RequestIYC003R;

    public class ResponseIYC003RI
    {
        public string STATUS { get; set; }
        public MensajeIYC003RI MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003RI
    {
        public List<datosIYC003RI> datos { get; set; }
    }

    public class datosIYC003RI
    {
        public string nro { get; set; }
        public string nit { get; set; }
        public string fecha { get; set; }
        public string id_declaracion { get; set; }
        public string tipo_form { get; set; }
        public string tiket { get; set; }
    }
}