namespace Domain.Entities.Epayco
{
    [Index(nameof(transactionID), IsUnique = true)]
    public class EPAYCO
    {
        [Key]
        public Guid id_pago { get; set; } = Guid.NewGuid();

        [Required]
        public int ref_payco { get; set; }

        [Required, MaxLength(200)]
        public string factura { get; set; }

        [MaxLength(200)]
        public string descripcion { get; set; }

        [Column(TypeName = "decimal(16, 2)")]
        public decimal valor { get; set; }

        [MaxLength(10)]
        public string estado { get; set; }

        [MaxLength(100)]
        public string transactionID { get; set; }

        public string token { get; set; }
    }
}
