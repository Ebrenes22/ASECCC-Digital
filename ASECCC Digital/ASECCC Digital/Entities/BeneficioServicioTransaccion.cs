using System;
using System.Collections.Generic;

namespace ASECCC_Digital.Entities
{
    public class BeneficioServicioTransaccion
    {
        public int TransaccionBeneficiosServiciosId { get; set; }  // Identificador único de la transacción
        public int UsuarioId { get; set; }  // Identificador del usuario que realiza el pago
        public int BeneficioId { get; set; }  // Identificador del beneficio o servicio al que se asocia el pago
        public decimal MontoTotal { get; set; }  // Monto total del beneficio o servicio
        public decimal MontoPendiente { get; set; }  // Monto pendiente de pago
        public string NumeroProforma { get; set; }  // Número de proforma asociado al pago
        public int? Plazo { get; set; }  // Plazo en semanas o meses para el pago
        public DateTime FechaCreacion { get; set; }  // Fecha de creación del pago
        public string Estado { get; set; }  // Estado de la transacción (activo o cancelado)

        // Propiedades de navegación
        public Usuario Usuario { get; set; }  // Relación con la entidad Usuario
        public BeneficioServicio BeneficioServicio { get; set; }  // Relación con la entidad BeneficioServicio
    }
}