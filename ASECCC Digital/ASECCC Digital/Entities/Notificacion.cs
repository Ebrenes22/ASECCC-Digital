using System;

namespace ASECCC_Digital.Entities
{
    public class Notificacion
    {
        public int NotificacionId { get; set; }  // Identificador único de la notificación
        public int UsuarioId { get; set; }  // Identificador del usuario que recibe la notificación
        public string Titulo { get; set; }  // Título de la notificación
        public string Contenido { get; set; }  // Contenido de la notificación
        public string Tipo { get; set; }  // Tipo de notificación (general o personalizada)
        public DateTime FechaEnvio { get; set; }  // Fecha de envío de la notificación
        public string Estado { get; set; }  // Estado de la notificación (enviada o leída)

        // Relación con la entidad Usuario
        public Usuario Usuario { get; set; }  // Relación con la entidad Usuario que recibe la notificación
    }
}