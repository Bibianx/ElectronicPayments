namespace Domain.Entities.ZonaPagos
{
    [Index(nameof(str_id_pago))]
    public class INTENTOSZP
    {
        [Key]
        public Guid id_pago { get; set; } = Guid.NewGuid();

        [Required, Column(TypeName = "decimal(16, 2)")]
        public decimal flt_total_con_iva { get; set; }

        [Column(TypeName = "decimal(16, 2)")]
        public decimal flt_valor_iva { get; set; }

        [Required, MaxLength(30)]
        public string str_id_pago { get; set; }

        [Required, MaxLength(70)]
        public string str_descripcion_pago { get; set; }

        [MaxLength(70)]
        public string str_email { get; set; }

        [MaxLength(30)]
        public string str_id_cliente { get; set; }

        [MaxLength(5)]
        public string str_tipo_id { get; set; }

        [MaxLength(50)]
        public string str_nombre_cliente { get; set; }

        [MaxLength(50)]
        public string str_apellido_cliente { get; set; }

        [MaxLength(50)]
        public string str_telefono_cliente { get; set; }

        [MaxLength(70)]
        public string str_opcional1 { get; set; }

        [MaxLength(70)]
        public string str_opcional2 { get; set; }

        [MaxLength(70)]
        public string str_opcional3 { get; set; }

        [MaxLength(70)]
        public string str_opcional4 { get; set; }

        [MaxLength(70)]
        public string str_opcional5 { get; set; }

        [Required]
        public int int_id_comercio { get; set; }

        [Required, MaxLength(40)]
        public string str_usuario { get; set; }

        [Required, MaxLength(50)]
        public string str_clave { get; set; }

        [Required]
        public int int_modalidad { get; set; }

        [Required]
        public int numero_ticket_cobol { get; set; }

        [MaxLength(2)]
        public string estado_intento { get; set; }
        public DateOnly? fecha_intento { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly? hora_intento { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    }
}
