using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class Aporte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AporteId { get; set; }  // Identificador único del aporte

        [Required(ErrorMessage = "El usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; } // Identificador del usuario que realiza el aporte

        [Required(ErrorMessage = "El tipo de aporte es requerido")]
        [RegularExpression("^(personal|patronal)$", ErrorMessage = "Tipo de aporte inválido")]
        [StringLength(50)]
        public string TipoAporte { get; set; }  // Tipo de aporte (personal o patronal)

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, 100000, ErrorMessage = "Monto fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; } // Monto del aporte

        [Required(ErrorMessage = "La fecha de registro es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; } // Fecha en la que se registra el aporte

        // Propiedad de navegación hacia la entidad Usuario
        public Usuario Usuario { get; set; }

        // Relación con las transacciones de aportes
        public ICollection<AporteTransaccion> AporteTransacciones { get; set; }
    }
}