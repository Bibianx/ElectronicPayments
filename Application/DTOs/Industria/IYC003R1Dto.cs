namespace Aplication.DTOs.Industria
{
    public class RequestIYC003R1
    {
        public string id_comercio { get; set; }
        public string sesion { get; set; }
        public string usuario { get; set; }
        public string llave { get; set; }
        public string recibo { get; set; }
        public string validar_rbo { get; set; }
    }

    public class ResponseIYC003R1
    {
        public string STATUS { get; set; }
        public MensajeIYC003R1 MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003R1
    {
        public string ano { get; set; }
        public string apeid { get; set; }
        public string autoret_ica { get; set; }
        public string avi_pago { get; set; }
        public string bom_pago { get; set; }
        public string ciudad { get; set; }
        public string correo { get; set; }
        public string direcc { get; set; }
        public string fecha_limit { get; set; }
        public string id { get; set; }
        public string imp_pago { get; set; }
        public string int_ad { get; set; }
        public string monto_int { get; set; }
        public string nit_usunet { get; set; }
        public string nomid { get; set; }
        public string nro_dias { get; set; }
        public string otro_pago { get; set; }
        public string pago_inter { get; set; }
        public string periodo { get; set; }
        public string razon { get; set; }
        public string ret_ica { get; set; }
        public string sancion_min { get; set; }
        public string telefono { get; set; }
        public string tipo_id { get; set; }
        public string total_impto { get; set; }
        public string fecha_vence { get; set; }
    }

}