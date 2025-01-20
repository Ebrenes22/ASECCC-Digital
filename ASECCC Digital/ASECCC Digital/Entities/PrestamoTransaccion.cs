using System;

namespace ASECCC_Digital.Entities
{
    public class PrestamoTransaccion
    {
        public int TransaccionPrestamoId { get; set; }  // Identificador único de la transacción
        public int PrestamoId { get; set; }  // Identificador del préstamo al que pertenece el pago
        public decimal MontoAbonado { get; set; }  // Monto abonado en el pago
        public DateTime FechaPago { get; set; }  // Fecha de la transacción del pago

        // Propiedad de navegación hacia la entidad Prestamo
        public Prestamo Prestamo { get; set; }
    }
}