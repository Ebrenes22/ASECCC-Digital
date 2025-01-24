using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class AporteTransaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransaccionAportesId { get; set; } // Identificador único de la transacción

        [Required(ErrorMessage = "El aporte es requerido")]
        [ForeignKey("Aporte")]
        public int AporteId { get; set; } // Identificador del aporte al que corresponde la transacción

        [Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(-100000, 100000, ErrorMessage = "Monto fuera de rango")]
        public decimal Monto { get; set; } // Monto de la transacción (positivo para aumentos, negativo para reducciones)

        [Required(ErrorMessage = "La fecha de transacción es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaTransaccion { get; set; }  // Fecha de la transacción

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; }  // Descripción o detalle de la transacción

        // Propiedad de navegación hacia la entidad Aporte
        public Aporte Aporte { get; set; }
    }
}