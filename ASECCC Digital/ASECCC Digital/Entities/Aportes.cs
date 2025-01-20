using System;
using System.Collections.Generic;

namespace ASECCC_Digital.Entities
{
    public class Aporte
    {
        public int AporteId { get; set; }  // Identificador único del aporte
        public int UsuarioId { get; set; }  // Identificador del usuario que realiza el aporte
        public string TipoAporte { get; set; }  // Tipo de aporte (personal o patronal)
        public decimal Monto { get; set; }  // Monto del aporte
        public DateTime FechaRegistro { get; set; }  // Fecha en la que se registra el aporte

        // Propiedad de navegación hacia la entidad Usuario
        public Usuario Usuario { get; set; }

        // Relación con las transacciones de aportes
        public ICollection<AporteTransaccion> AporteTransacciones { get; set; }
    }
}