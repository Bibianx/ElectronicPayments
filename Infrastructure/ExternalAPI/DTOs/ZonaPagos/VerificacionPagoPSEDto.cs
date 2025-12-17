namespace Infraestructure.ExternalAPI.DTOs.ZonaPagos
{
    public class VerificacionPagoPSEParams
    {
        public int int_id_comercio { get; set; }
        public string str_usr_comercio { get; set; }
        public string str_pwd_Comercio { get; set; }
        public string str_id_pago { get; set; }
        public int int_no_pago { get; set; }
    }

    public class VerificacionPagoPSEResponse
    {
        public int int_estado { get; set; }
        public int int_error { get; set; }
        public string str_detalle { get; set; }
        public int int_cantidad_pagos { get; set; }
        public string str_res_pago { get; set; }
    }
}