namespace Domain.Entities.Recaudos
{
    [Index(nameof(codigo_recaudo))]
    public class RECAUDOSBANCOS
    {
        [Key]
        public Guid id_recaudo { get; set; } = Guid.NewGuid();

        [Required, MaxLength(30)]
        public string codigo_recaudo { get; set; }

        [Required, MaxLength(10)]
        public string id_transaccion { get; set; }

        [Required, Column(TypeName = "decimal(16, 2)")]
        public decimal total_recaudo { get; set; }

        [Column(TypeName = "jsonb")]
        public string json_notificacion_recaudo { get; set; }

        public bool es_reversado { get; set; } = false;
        public ComercioRecaudo codigo_comercio_recaudo { get; set; }
        public ProveedorRecaudo codigo_proveedor_recaudo { get; set; }
        public EstadoRecaudo codigo_estado_recaudo { get; set; } = EstadoRecaudo.Pendiente;
        public DateTime fecha_recaudo { get; set; } = DateTime.UtcNow;
        public DateTime? fecha_reverso_recaudo { get; set; }
        public string estado_recaudo_cobol { get; set; }
        public string detalle_recaudo_cobol { get; set; }
        public bool? recaudo_contabilizado_ok { get; set; }
        public bool? ticket_cobol_ok { get; set; }
    }

    public enum ComercioRecaudo
    {
        ComercializadoraDelLlano, //0
        MunicipioVillanueva, //1
        MunicipioMonterrey, //2
    }

    public enum ProveedorRecaudo
    {
        Davivienda, //0
        ZonaPagos, //1
        BancoBogota, //2
    }

    public enum EstadoRecaudo
    {
        Aprobado, //0
        Pendiente, //1
        Rechazado, //2
        Reversado, //3
    }
}
