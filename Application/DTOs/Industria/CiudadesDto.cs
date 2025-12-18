namespace Aplication.DTOs.Industria
{
     public class RequestDLLCiudades
    {
        public string sesion { get; set; }
    }

    public class ResponseDLLCiudades
    {
        public string STATUS { get; set; }
        public MensajeDLLCiudades MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeDLLCiudades
    {
        public List<Ciudades> ciudades { get; set; }
    }

    public class Ciudades
    {
        public string dpto { get; set; }
        public string ciudad { get; set; }
        public string nombre { get; set; }
    }

}