using System;
using System.Collections.Generic;

namespace ASECCC_Digital.Entities
{
    public class Prestamo
    {
        public int PrestamoId { get; set; }  // Identificador único del préstamo
        public int UsuarioId { get; set; }  // Identificador del usuario que solicita el préstamo
        public decimal MontoSolicitado { get; set; }  // Monto solicitado en el préstamo
        public decimal? MontoAprobado { get; set; }  // Monto aprobado para el préstamo
        public int Plazo { get; set; }  // Plazo del préstamo en semanas
        public decimal? CuotaSemanal { get; set; }  // Cuota semanal calculada
        public string TipoPrestamo { get; set; }  // Tipo de préstamo (urgente, personal, 150%)
        public string Estado { get; set; }  // Estado del préstamo (solicitado, aprobado, rechazado, activo)
        public DateTime FechaSolicitud { get; set; }  // Fecha en la que se solicita el préstamo
        public DateTime? FechaAprobacion { get; set; }  // Fecha en la que se aprueba el préstamo
        public decimal? SaldoPendiente { get; set; }  // Saldo pendiente por pagar

        // Propiedad de navegación hacia la entidad Usuario
        public Usuario Usuario { get; set; }

        // Relación con las transacciones de préstamo
        public ICollection<PrestamoTransaccion> PrestamoTransacciones { get; set; }
    }
}