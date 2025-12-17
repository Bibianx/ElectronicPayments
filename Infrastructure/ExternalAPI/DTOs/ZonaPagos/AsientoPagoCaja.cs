namespace Infraestructure.ExternalAPI.DTOs.ZonaPagos
{
    public class AsientoPagoCajaParams : ConsultaPagoCajaParams
    {
        public string Fecha_pago { get; set; }
        public string Id_transaccion { get; set; }
        public decimal Valor_pagado { get; set; }
    }

    public class AsientoPagoCajaDto
    {
        public string Codigo_estado { get; set; }
        public string Severidad { get; set; }
        public string Descripcion { get; set; }
    }
}