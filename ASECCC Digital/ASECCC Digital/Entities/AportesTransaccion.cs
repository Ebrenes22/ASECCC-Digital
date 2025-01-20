using System;

namespace ASECCC_Digital.Entities
{
    public class AporteTransaccion
    {
        public int TransaccionAportesId { get; set; }  // Identificador único de la transacción
        public int AporteId { get; set; }  // Identificador del aporte al que corresponde la transacción
        public decimal Monto { get; set; }  // Monto de la transacción (positivo para aumentos, negativo para reducciones)
        public DateTime FechaTransaccion { get; set; }  // Fecha de la transacción
        public string Descripcion { get; set; }  // Descripción o detalle de la transacción

        // Propiedad de navegación hacia la entidad Aporte
        public Aporte Aporte { get; set; }
    }
}