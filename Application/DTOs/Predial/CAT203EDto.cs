namespace Aplication.DTOs.Predial
{
    public class CAT203ERequest
    {
        public string nro_cat { get; set; }
        public string usuario { get; set; }
        public string ano { get; set; }
    }
    public class CAT203EResponse
    {
        public string STATUS { get; set; }
        public CAT203EMensaje MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class CAT203EMensaje
    {
        public string nro_cat { get; set; }
        public string anohasta { get; set; }
        public string id { get; set; }
        public string impuesto { get; set; }
        public string descuento { get; set; }
        public string interes { get; set; }
        public string neto { get; set; }
        public string fecha { get; set; }
        public string propietario { get; set; }
        public string direccion  { get; set; }
        public string direcc_noti { get; set; }
        public string email_noti { get; set; }
        public string tipo_id_prop { get; set; }
        public string nit_usunet { get; set; }
    }
}