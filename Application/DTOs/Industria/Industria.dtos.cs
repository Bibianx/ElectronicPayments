namespace Aplication.DTOs.Industria
{
    public class RequestIYC006G
    {
        public string sesion { get; set; }
        public string usuario { get; set; }
        public string llave_imp { get; set; }
        public string recibo { get; set; }
    }

    public class RequestIYC007 : RequestIYC006G
    {
        public string estado { get; set; }
    }

    public class RequestIYC005
    {
        public string usuario { get; set; }
        public string sesion { get; set; }
        public string recibo { get; set; }
        public string ticket { get; set; }
        public string paso { get; set; }
    }

    public class EstructuraResponseDLL
    {
        public string STATUS { get; set; }
        public string MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class RequestIYC003R
    {
        public string sesion { get; set; }
        public string usuario { get; set; }
        public int tipo_form { get; set; } // 1: declaracion bimestral, 2: declaracion anual
    }

    public class ResponseIYC003R
    {
        public string STATUS { get; set; }
        public MensajeIYC003R MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003R
    {
        public List<DatoIYC003R> datos { get; set; }
    }

    public class DatoIYC003R
    {
        public string nit { get; set; }
        public string periodo { get; set; }
        public string ficha { get; set; }
        public string fecha_creacion { get; set; }
        public string estado { get; set; }
        public string abonos { get; set; }
        public string total_fact { get; set; }
        public string ticket { get; set; }
        public string saldo_fact { get; set; }
    }

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

    public class IYC003RGRequest
    {
        public string sesion { get; set; }
        public string usuario { get; set; }
        public string llave { get; set; }
        public decimal imp_pago { get; set; }
        public decimal avi_pago { get; set; }
        public decimal bom_pago { get; set; }
        public decimal otro_pago { get; set; }
        public decimal impuestos { get; set; }
        public decimal sancion { get; set; }
        public decimal interes { get; set; }
    }

    public class RequestIYC003RP
    {
        public string usuario { get; set; }
        public string sesion { get; set; }
        public string nro_pago { get; set; }
    }

    public class RequestIYC003 : RequestIYC003R;

    public class ResponseIYC003
    {
        public string STATUS { get; set; }
        public MensajeIYC003 MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003
    {
        public List<DatoIYC003> datos { get; set; }
    }

    public class DatoIYC003
    {
        public string nit { get; set; }
        public string periodo { get; set; }
        public string ficha { get; set; }
        public string fecha_creacion { get; set; }
        public string saldo_favor { get; set; }
        public string estado { get; set; }
        public string tipo_form { get; set; }
        public string reg_iva { get; set; }
        public string modific { get; set; }
    }

    public class ADM005Request
    {
        public string cedula { get; set; }
    }

    public class ADM005Response : EstructuraResponseDLL;

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

    public class RequestIYC002I : RequestIYC002A
    {
        public string llave { get; set; }
    }

    public class ResponseIYC002I
    {
        public string STATUS { get; set; }
        public MensajeIYC002I MENSAJE { get; set; } = new();
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC002I
    {
        public EncabezadoIYC002I encabezado { get; set; } = new();
        public InfoTerceroIYC002I info_tercero { get; set; } = new();
        public BaseGravableIYC002I base_gravable { get; set; } = new();
        public List<ActividadGravadaIYC002I> act_gravadas { get; set; } = new();
        public LiquidacionIYC002I liquidacion { get; set; } = new();
        public TotalesIYC002I totales { get; set; } = new();
        public PagoIYC002I pago { get; set; } = new();
    }

    public class EncabezadoIYC002I
    {
        public long nit { get; set; }
        public string nombre { get; set; }
        public string anio_gravable { get; set; }
        public string fecha_pago { get; set; }
        public string nro_declara { get; set; }
        public string gran_contri { get; set; }
        public string correcion { get; set; }
        public string estado { get; set; }
        public string marca { get; set; }
    }

    public class InfoTerceroIYC002I
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public string tipo_id { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string regimen { get; set; }
    }

    public class BaseGravableIYC002I
    {
        public decimal ing_bruto { get; set; }
        public decimal menos_ing { get; set; }
        public decimal total_ing_act { get; set; }
        public decimal desc_devol_4056 { get; set; }
        public decimal desc_expor_4056 { get; set; }
        public decimal desc_act_fijos { get; set; }
        public decimal desc_otras_4056 { get; set; }
        public decimal exenciones { get; set; }
        public string total_base { get; set; }
    }

    public class ActividadGravadaIYC002I
    {
        public string cod_ciiu { get; set; }
        public string cod { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("base")]
        public decimal base_ { get; set; }
        public decimal tarifa { get; set; }
        public decimal vlr_impto { get; set; }
    }

    public class LiquidacionIYC002I
    {
        public decimal impto { get; set; }
        public decimal avisos { get; set; }
        public decimal adicionales { get; set; }
        public decimal sobretasa { get; set; }
        public decimal seguridad { get; set; }
        public decimal energia { get; set; }
        public string total { get; set; }
        public decimal exenciones_ica { get; set; }
        public decimal desc_reten_4056 { get; set; }
        public decimal desc_autoreten_4056 { get; set; }
        public decimal ant_ant { get; set; }
        public decimal ant_prx { get; set; }
        public decimal sancion { get; set; }
        public string sdo_anter { get; set; }
        public string total_saldo { get; set; }
    }

    public class TotalesIYC002I
    {
        public decimal descuentos { get; set; }
        public decimal mora { get; set; }
        public string sdo_favor { get; set; }
        public string total { get; set; }
    }

    public class PagoIYC002I
    {
        public string representante { get; set; }
        public string tipo { get; set; }
        public string id_repre { get; set; }
        public string fir_rep { get; set; }
        public string fec_rep { get; set; }
        public string fiscal { get; set; }
        public string nom_fis { get; set; }
        public string fir_fis { get; set; }
        public string fec_fis { get; set; }
        public string contad { get; set; }
        public string nom_con { get; set; }
        public string fir_con { get; set; }
        public string fec_con { get; set; }
        public string convenio { get; set; }
        public string nro_decl { get; set; }
        public string nro_pag { get; set; }
        public decimal vlr_saldo { get; set; }
        public string fecha { get; set; }
        public string arbat { get; set; }
    }

    public class RequestIYC003RI : RequestIYC003R;

    public class ResponseIYC003RI
    {
        public string STATUS { get; set; }
        public MensajeIYC003RI MENSAJE { get; set; }
        public string PROGRAM { get; set; }
    }

    public class MensajeIYC003RI
    {
        public List<datosIYC003RI> datos { get; set; }
    }

    public class datosIYC003RI
    {
        public string nro { get; set; }
        public string nit { get; set; }
        public string fecha { get; set; }
        public string id_declaracion { get; set; }
        public string tipo_form { get; set; }
        public string tiket { get; set; }
    }
}
