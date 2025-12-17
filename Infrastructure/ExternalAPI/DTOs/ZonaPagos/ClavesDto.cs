namespace Infraestructure.ExternalAPI.DTOs.ZonaPagos
{
    //Para capturar a traves de IOptions las claves de ZonaPagos de los distintos comercios
    public abstract class ClavesBase
    {
        public string StrUsrComercio { get; set; }
        public string StrPwdComercio { get; set; }
        public int IntIdComercio { get; set; }
    }

    public class ClavesCAJAZP : ClavesBase
    {
        public int IntIdBanco { get; set; }
        public string IpVillanueva { get; set; }
    }

    public class ClavesPSE : ClavesBase
    {
        public string IpVillanueva { get; set; }
    }

    public class ClavesMonterrey : ClavesBase
    {
        public int IntIdBanco { get; set; }
        public string IpMonterrey { get; set; }
    }

    //Para pasar las credenciales a los metodos de ZonaPagos
    public class ZonaPagosCredentials
    {
        public int int_id_comercio { get; set; }
        public string str_usr_comercio { get; set; }
        public string str_pwd_Comercio { get; set; }
        public string str_id_pago { get; set; }
        public string int_no_pago { get; set; }
    }
}