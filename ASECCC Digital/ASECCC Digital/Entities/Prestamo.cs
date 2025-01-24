using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class Prestamo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrestamoId { get; set; }  // Identificador único del préstamo.

        [Required(ErrorMessage = "El usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }  // Clave foránea que referencia al usuario que solicitó el préstamo.

        [Range(0.01, 1000000, ErrorMessage = "Monto aprobado fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MontoAprobado { get; set; }  // Monto aprobado para el préstamo, puede ser nulo si aún no se ha aprobado.

        [Required(ErrorMessage = "El plazo es requerido")]
        [Range(1, 120, ErrorMessage = "Plazo debe estar entre 1 y 120 semanas")]
        public int Plazo { get; set; }  // Plazo del préstamo en semanas.

        [Range(0.01, 100000, ErrorMessage = "Cuota semanal fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CuotaSemanal { get; set; }  // Cuota semanal que debe pagar el usuario, puede ser nula si aún no se ha calculado.

        [Required(ErrorMessage = "El tipo de préstamo es requerido")]
        [RegularExpression("^(urgente|personal|150%)$", ErrorMessage = "Tipo de préstamo inválido")]
        [StringLength(50)]
        public string TipoPrestamo { get; set; }  // Tipo de préstamo: urgente, personal o 150%.

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(activo|cancelado)$", ErrorMessage = "Estado inválido")]
        [StringLength(50)]
        public string EstadoPrestamo { get; set; }  // Estado del préstamo: activo o cancelado.

        [Required(ErrorMessage = "La fecha de solicitud es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaSolicitud { get; set; }  // Fecha y hora en que el préstamo fue solicitado.

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEstado { get; set; }  // Fecha en que se aprueba o cambia el estado del préstamo.

        [Range(0, 1000000, ErrorMessage = "Saldo pendiente fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SaldoPendiente { get; set; }  // Saldo pendiente de pago del préstamo, puede ser nulo si no hay saldo.

        // Propiedad de navegación para la relación con la entidad Usuario
        public virtual Usuario Usuario { get; set; }  // Relación con el Usuario que solicita el préstamo

        // Colección de transacciones asociadas al préstamo, si es necesario en la lógica de negocio
        public ICollection<PrestamoTransaccion> PrestamoTransacciones { get; set; }  // Transacciones asociadas al préstamo (si aplica)
    }
}
