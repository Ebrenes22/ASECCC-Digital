using System;
using System.Collections.Generic;

namespace ASECCC_Digital.Entities
{
    public class Ahorro
    {
        public int AhorroId { get; set; }  // Identificador único del ahorro
        public int UsuarioId { get; set; }  // Identificador del usuario que realiza el ahorro
        public int TipoAhorroId { get; set; }  // Referencia al catálogo de tipos de ahorro
        public decimal MontoInicial { get; set; }  // Monto inicial del ahorro
        public decimal MontoActual { get; set; }  // Monto actual acumulado
        public DateTime FechaInicio { get; set; }  // Fecha de inicio del ahorro
        public int? Plazo { get; set; }  // Plazo del ahorro en semanas o meses
        public string Estado { get; set; }  // Estado del ahorro (activo o cancelado)

        // Navegación a la entidad Usuario
        public Usuario Usuario { get; set; }

        // Relación con las transacciones de ahorro
        public ICollection<AhorroTransaccion> AhorroTransacciones { get; set; }

        // Navegación a la entidad CatalogoTipoAhorro es de consulta solamente

    }
}