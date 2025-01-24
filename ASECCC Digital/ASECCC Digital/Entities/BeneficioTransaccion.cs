using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class BeneficioTransaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransaccionId { get; set; }  // Identificador único de la transacción

        [Required(ErrorMessage = "La cuenta de beneficios es requerida")]
        [ForeignKey("BeneficiosServiciosCuenta")]
        public int CuentaBeneficiosServiciosId { get; set; } // Identificador único de la cuenta a la que pertenece la transaccion

        [Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(-100000, 100000, ErrorMessage = "Monto fuera de rango")]
        public decimal Monto { get; set; } // Monto de la transacción.

        [Required(ErrorMessage = "La fecha de transacción es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaTransaccion { get; set; } = DateTime.Now;  // Fecha y hora de la transacción, por defecto es la fecha actual.

        [StringLength(500, ErrorMessage = "Descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } // Descripción opcional de la transacción.

        // Propiedad de navegación que establece la relación con la entidad "BeneficioServicioCuenta".
        public virtual BeneficioServicioCuenta BeneficiosServiciosCuenta { get; set; }
    }
}