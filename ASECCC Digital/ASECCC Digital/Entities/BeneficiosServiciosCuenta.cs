using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class BeneficioServicioCuenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CuentaBeneficiosServiciosId { get; set; } // // Identificador único de la cuenta a cobrar

        [Required(ErrorMessage = "El usuario es requerido")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; } // Identificador del usuario al que pertenece la cuenta

        [Required(ErrorMessage = "El beneficio es requerido")]
        [ForeignKey("BeneficioServicio")]
        public int BeneficioId { get; set; } // Identificador único del beneficio o servicio

        [Required(ErrorMessage = "El monto total es requerido")]
        [Range(0.01, 1000000, ErrorMessage = "Monto total fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoTotal { get; set; } // Monto total de la cuenta a cobrar

        [Required(ErrorMessage = "El monto pendiente es requerido")]
        [Range(0, 1000000, ErrorMessage = "Monto pendiente fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoPendiente { get; set; } // Monto pendiente de la cuenta a cobrar

        [StringLength(50, ErrorMessage = "Número de proforma no puede exceder 50 caracteres")]
        public string NumeroProforma { get; set; } // Numero de proforma del documento

        [Range(1, 100, ErrorMessage = "Plazo debe estar entre 1 y 100 semanas")]
        public int? Plazo { get; set; } //Plazo en meses

        [Required(ErrorMessage = "La fecha de creación es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now; //Fecha de creacion de la cuenta

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(activo|cancelado)$", ErrorMessage = "Estado inválido")]
        [StringLength(50)]
        public string Estado { get; set; } //Estado de la cuenta

        // Propiedades de navegación
        public Usuario Usuario { get; set; }
        public BeneficioServicio BeneficioServicio { get; set; }
    }
}