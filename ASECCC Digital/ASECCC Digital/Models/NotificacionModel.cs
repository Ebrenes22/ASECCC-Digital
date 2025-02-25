using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;

namespace ASECCC_Digital.Models
{
    public class NotificacionModel
    {


        public List<Database.Notificaciones> ObtenerNotificacionesGenerales()
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                return context.Notificaciones
                              .Where(n => n.tipo.ToLower() == "general") // Filtra solo las notificaciones de tipo "General"
                              .OrderByDescending(n => n.fechaEnvio) // Ordena por fecha descendente (las más recientes primero)
                              .ToList();
            }
        }

    }
}