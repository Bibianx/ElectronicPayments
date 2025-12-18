namespace Aplication.DTOs.Industria
{
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
        public List<ActividadGravadaIYC002I> act_gravadas { get; set; } = [];
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

}