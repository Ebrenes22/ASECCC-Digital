using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASECCC_Digital.Entities
{
    public class PrestamoTransaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransaccionPrestamoId { get; set; }  // Identificador único de la transacción

        [Required(ErrorMessage = "El identificador del préstamo es requerido")]
        [ForeignKey("Prestamo")]
        public int PrestamoId { get; set; }  // Identificador del préstamo al que pertenece el pago

        [Required(ErrorMessage = "El monto abonado es requerido")]
        [Range(0.01, 1000000, ErrorMessage = "Monto abonado fuera de rango")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoAbonado { get; set; }  // Monto abonado en el pago

        [Required(ErrorMessage = "La fecha de pago es requerida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaPago { get; set; }  // Fecha de la transacción del pago

        // Propiedad de navegación hacia la entidad Prestamo
        public Prestamo Prestamo { get; set; }
    }
}
