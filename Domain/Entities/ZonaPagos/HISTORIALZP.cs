using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.ZonaPagos
{
    [Index(nameof(id_pago))]
    public class HISTORIALZP
    {
        [Key]
        public Guid id_historial { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public string cod_estado_ini { get; set; }

        [MaxLength(50)]
        public string descrip_estado_ini { get; set; }

        [MaxLength(50)]
        public string cod_estado_fin { get; set; }

        [MaxLength(50)]
        public string descrip_estado_fin { get; set; }

        [MaxLength(255)]
        public string detalle_pago { get; set; }

        [MaxLength(50)]
        public string origen_cambio { get; set; }

        [Column(TypeName = "jsonb")]
        public string json_respuesta { get; set; }
        public DateOnly? fecha_ini { get; set; }
        public DateOnly? fecha_fin { get; set; }
        public TimeOnly? hora_ini { get; set; }
        public TimeOnly? hora_fin { get; set; }
        public bool? contabilizado_cobol_ok { get; set; }
        public bool? ticket_cobol_ok { get; set; }
        public string estado_cobol_ini { get; set; }
        public string detalle_cobol_ini { get; set; }
        public string estado_cobol_fin { get; set; }
        public string detalle_cobol_fin { get; set; }
        public Guid id_pago { get; set; }

        [ForeignKey("id_pago")]
        public INTENTOSZP intentos_zp { get; set; }
    }
}
