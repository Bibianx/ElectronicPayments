namespace Infraestructure.ExternalAPI.DTOs.ZonaPagos
{
    public class InicioPagoPSEParams
    {
        public InformacionPagoDto InformacionPago { get; set; }
        public InformacionSeguridadDto InformacionSeguridad { get; set; }
        public List<AdicionalPagoDto> AdicionalesPago { get; set; }
        public List<AdicionalConfiguracionDto> AdicionalesConfiguracion { get; set; }
    }

    public class InformacionUsuarioDto
    {
        public string usuario { get; set; }
        public string sesion { get; set; }
        public string llave_imp { get; set; }
    }

    public class InformacionPagoDto
    {
        public decimal flt_total_con_iva { get; set; }
        public decimal flt_valor_iva { get; set; }
        public string str_id_pago { get; set; }
        public string str_descripcion_pago { get; set; }
        public string str_email { get; set; }

        //cliente registrado en pse y a nombre de quien se realiza el pago
        public string str_id_cliente { get; set; }

        //1.CC 2.CE 3.NIT 4.NUIP 5.TI 6.PP 7.IDC 8.CEL 9.RC 10.DE 11.OTRO
        public string str_tipo_id { get; set; }
        public string str_nombre_cliente { get; set; }
        public string str_apellido_cliente { get; set; }
        public string str_telefono_cliente { get; set; }
        public string str_opcional1 { get; set; }
        public string str_opcional2 { get; set; }
        public string str_opcional3 { get; set; }
        public string str_opcional4 { get; set; }
        public string str_opcional5 { get; set; }
    }

    public class InformacionSeguridadDto
    {
        public int int_id_comercio { get; set; }
        public string str_usuario { get; set; }
        public string str_clave { get; set; }
        public int int_modalidad { get; set; } // siempre se debe enviar -1
    }

    public class AdicionalPagoDto
    {
        public int int_codigo { get; set; }
        public string str_valor { get; set; }
    }

    public class AdicionalConfiguracionDto
    {
        public int int_codigo { get; set; }
        public string str_valor { get; set; }
    }

    public class InicioPagoResponsePSEDto
    {
        public int int_codigo { get; set; }
        public string str_cod_error { get; set; }
        public string str_descripcion_error { get; set; }
        public string str_url { get; set; }
    }
}