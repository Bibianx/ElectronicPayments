namespace Aplication.DTOs.Industria
{
    public class ADM005Request
    {
        public string cedula { get; set; }
    }

    public class ADM005Response : EstructuraResponseDLL;
    
    public class EstructuraResponseDLL
    {
        public string STATUS { get; set; }
        public string MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }
}
