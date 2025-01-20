using System;

namespace ASECCC_Digital.Entities
{
    public class AhorroTransaccion
    {
        public int TransaccionAhorroId { get; set; }  // Identificador único de la transacción
        public int AhorroId { get; set; }  // Identificador del ahorro al que pertenece la transacción
        public int TipoTransaccionId { get; set; }  // Referencia al catálogo de tipos de transacción
        public decimal Monto { get; set; }  // Monto de la transacción (positivo para depósitos, negativo para retiros)
        public DateTime FechaTransaccion { get; set; }  // Fecha de la transacción
        public string Descripcion { get; set; }  // Descripción o detalle de la transacción

        // Navegación a la entidad Ahorro
        public Ahorro Ahorro { get; set; }

        // Navegación a la entidad CatalogoTipoTransaccion es de consulta solamente
    }
}