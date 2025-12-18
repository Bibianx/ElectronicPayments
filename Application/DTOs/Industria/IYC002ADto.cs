namespace Aplication.DTOs.Industria
{
    public class RequestIYC002A
    {
        public string usuario { get; set; }
        public string sesion { get; set; }
    }
    
    public class ResponseIYC002A
    {
        public string STATUS { get; set; }
        public MensajeIYC002A MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC002A
    {
        public string f_ins { get; set; }
        public string id { get; set; }
        public string pers { get; set; }
        public string tipo_id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("1ape")]
        public string Ape1 { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("2ape")]
        public string Ape2 { get; set; }

        public string nombre { get; set; }
        public string razon { get; set; }
        public string nit_rep { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("1ape_rep")]
        public string Ape1_rep { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("2ape_rep")]
        public string Ape2_rep { get; set; }

        public string nom_rep { get; set; }
        public string contado { get; set; }
        public string nom_con { get; set; }
        public string rev_fis { get; set; }
        public string nom_fis { get; set; }
        public string direcc { get; set; }
        public string correo { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string ciudad { get; set; }

        public string ciiu1 { get; set; }
        public string ciiu2 { get; set; }
        public string ciiu3 { get; set; }
        public string ciiu4 { get; set; }
        public string ciiu5 { get; set; }
        public string ciiu6 { get; set; }
        public string ciiu7 { get; set; }
        public string ciiu8 { get; set; }
        public string ciiu9 { get; set; }
        public string ciiu10 { get; set; }

        public List<EstablecimientoIYC002A> establecimientos { get; set; }

        public string f_fin { get; set; }
        public string obs { get; set; }
        public string rind { get; set; }
        public string rret { get; set; }
        public string rexo { get; set; }
        public string reg_iva { get; set; }
        public string gran { get; set; }
        public string sor { get; set; }
        public string periodo { get; set; }
        public string mesas { get; set; }
        public string auto_ica { get; set; }
        public string fecha_limit { get; set; }
        public string pass { get; set; }
        public string ener { get; set; }
    }

    public class EstablecimientoIYC002A
    {
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string correo { get; set; }
        public string tel { get; set; }
        public string tipo_act { get; set; }
        public string mat { get; set; }
        public string desc { get; set; }
        public string f_matri { get; set; }
        public string f_inicio { get; set; }
    }

}