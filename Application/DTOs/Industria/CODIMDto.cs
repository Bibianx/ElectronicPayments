namespace Aplication.DTOs.Industria
{
    public class RequestCODIMP
    {
        public string sesion { get; set; }
    }

    public class ResponseCODIMP
    {
        public string STATUS { get; set; }
        public MensajeCODIMP MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeCODIMP
    {
        public List<ActividadCODIMP> codimp { get; set; }
    }

    public class ActividadCODIMP
    {
        public string cod { get; set; }
        public string DESCRIPCION { get; set; }
    }
}