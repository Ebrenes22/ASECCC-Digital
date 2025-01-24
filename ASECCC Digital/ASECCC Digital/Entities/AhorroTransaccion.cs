using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class AhorroTransaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransaccionAhorroId { get; set; }  // Identificador único de la transacción

        [Required(ErrorMessage = "El ahorro es requerido")]
        [ForeignKey("Ahorro")]
        public int AhorroId { get; set; }  // Identificador del ahorro al que pertenece la transacción

        [Required(ErrorMessage = "El tipo de transacción es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de transacción válido")]
        public int TipoTransaccionId { get; set; } // Referencia al catálogo de tipos de transacción

        [Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(-100000, 100000, ErrorMessage = "Monto fuera de rango")]
        public decimal Monto { get; set; } // Monto de la transacción 

        [Required(ErrorMessage = "La fecha de transacción es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)] 
        public DateTime FechaTransaccion { get; set; } // Fecha de la transacción

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } // Descripción o detalle de la transacción

        // Navegación a la entidad Ahorro
        public Ahorro Ahorro { get; set; }
    }
}