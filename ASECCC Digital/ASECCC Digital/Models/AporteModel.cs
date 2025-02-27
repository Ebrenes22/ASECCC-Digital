using System;
using System.ComponentModel.DataAnnotations;

namespace ASECCC_Digital.Models
{
    public class AporteModel
    {
        public int AporteId { get; set; }

        [Required(ErrorMessage = "El usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El tipo de aporte es requerido")]
        public string TipoAporte { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
