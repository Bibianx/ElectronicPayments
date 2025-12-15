using System.Text.Json.Serialization;

namespace Aplication.DTOs.ZonaPagos
{
    public class ConsultaPagoParams
    {
        public int Id_Comercio { get; set; }
        public string Password { get; set; }
        public int Id_Banco { get; set; }
        public string Referencia_pago { get; set; }
        public string Info_Adicional { get; set; }
    }

    public class ConsultaPagoDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Referencia_factura { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Valor_referencia_factura { get; set; }
        public string Fecha_limite_pago { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Valor_factura { get; set; }
        public string Codigo_Estado { get; set; }
        public string Descripcion_estado { get; set; }
        public string Info_Adicional { get; set; }
    }

    public class AsientoPagoParams : ConsultaPagoParams
    {
        public string Fecha_pago { get; set; }
        public string Id_transaccion { get; set; }
        public decimal Valor_pagado { get; set; }
    }

    public class AsientoPagoDto
    {
        public string Codigo_estado { get; set; }
        public string Severidad { get; set; }
        public string Descripcion { get; set; }
    }
}
