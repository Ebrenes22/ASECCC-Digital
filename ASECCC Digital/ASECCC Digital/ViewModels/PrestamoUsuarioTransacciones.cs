using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASECCC_Digital.Entities; // Asegúrate de importar las entidades necesarias

namespace ASECCC_Digital.ViewModels
{
    public class PrestamoUsuarioTransaccionesViewModel
    {
        // Datos del Usuario
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }

        // Lista de Préstamos con sus transacciones
        public List<PrestamoDetalleViewModel> ListaPrestamos { get; set; } = new List<PrestamoDetalleViewModel>();

        // Transacción Nueva
        public PrestamoTransaccionViewModel NuevaTransaccion { get; set; } = new PrestamoTransaccionViewModel();
    }

    public class PrestamoDetalleViewModel
    {
        public int PrestamoId { get; set; }
        public decimal? MontoAprobado { get; set; }
        public int Plazo { get; set; }
        public decimal? CuotaSemanal { get; set; }
        public string TipoPrestamo { get; set; }
        public string EstadoPrestamo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public decimal? SaldoPendiente { get; set; }

        // Transacciones del préstamo específico
        public List<PrestamoTransaccionViewModel> Transacciones { get; set; } = new List<PrestamoTransaccionViewModel>();
    }

    public class PrestamoTransaccionViewModel
    {
        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "El monto abonado es requerido")]
        [Range(0.01, 1000000, ErrorMessage = "Monto abonado fuera de rango")]
        public decimal MontoAbonado { get; set; }

        [Required(ErrorMessage = "La fecha de pago es requerida")]
        public DateTime FechaPago { get; set; }
    }
}
