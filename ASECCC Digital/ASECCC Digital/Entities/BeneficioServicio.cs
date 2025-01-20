using System;
using System.Collections.Generic;

namespace ASECCC_Digital.Entities
{
    public class BeneficioServicio
    {
        public int BeneficioId { get; set; }  // Identificador único del beneficio o servicio
        public string Nombre { get; set; }  // Nombre del beneficio o servicio
        public string Descripcion { get; set; }  // Descripción detallada del beneficio o servicio
        public string Categoria { get; set; }  // Categoría del beneficio o servicio
        public string Requisitos { get; set; }  // Requisitos para acceder al beneficio o servicio
        public string Estado { get; set; }  // Estado del beneficio o servicio (activo o inactivo)
        public DateTime FechaRegistro { get; set; }  // Fecha en la que se registra el beneficio o servicio

        // Relación con las transacciones de beneficios o servicios
        public ICollection<BeneficioServicioTransaccion> BeneficioServicioTransacciones { get; set; }
    }
}