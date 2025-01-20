using System;

namespace ASECCC_Digital.Entities
{
    public class SeguridadAuditoria
    {
        public int AuditoriaId { get; set; }  // Identificador único de la auditoría
        public int? UsuarioId { get; set; }  // Identificador del usuario que realizó la acción (puede ser null)
        public string Accion { get; set; }  // Descripción de la acción realizada
        public DateTime FechaAccion { get; set; }  // Fecha de la acción realizada
        public string DireccionIp { get; set; }  // Dirección IP del usuario que realizó la acción
        public string TipoAccion { get; set; }  // Tipo de acción (login, logout, modificación, consulta)

        // Relación con la entidad Usuario (opcional, ya que puede ser null)
        public Usuario Usuario { get; set; }  // Relación con la entidad Usuario (si el usuario está presente)
    }
}