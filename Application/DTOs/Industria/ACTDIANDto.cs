namespace Aplication.DTOs.Industria
{
    public class RequestACTDIAN
    {
        public string sesion { get; set; }
    }

    public class ResponseACTDIAN
    {
        public string STATUS { get; set; }
        public MensajeACTDIAN MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeACTDIAN
    {
        public string fecha_actualizacion { get; set; }
        public List<ActividadDIAN> actdian { get; set; }
    }

    public class ActividadDIAN
    {
        public string cod { get; set; }
        public string DESCRIPCION { get; set; }
        public string cod_munic { get; set; }
    }
}