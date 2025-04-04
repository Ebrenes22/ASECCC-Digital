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

        public List<Database.Notificaciones> ObtenerNotificacionesPersonalizadas()
        {
            using (var context = new Database.ASECCC_DIGITALEntities())
            {
                int usuarioId = Convert.ToInt32(System.Web.HttpContext.Current.Session["usuarioId"]);

                return context.Notificaciones
                              .Where(n => n.usuarioId == usuarioId)
                              .OrderByDescending(n => n.fechaEnvio)
                              .ToList();
            }
        }


        public List<Notificaciones> ObtenerNoLeidas()
        {
            try
            {
                using (var context = new ASECCC_DIGITALEntities())
                {
                    return context.Notificaciones
                                  .Where(n => n.estado.ToLower() == "enviado" && n.tipo.ToLower() == "general")
                                  .OrderByDescending(n => n.fechaEnvio)
                                  .Take(5)
                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ObtenerNoLeidas(): " + ex.Message);
            }
        }


        public void MarcarTodasComoLeidas()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var enviadas = context.Notificaciones
                                      .Where(n => n.estado.ToLower() == "enviado" && n.tipo.ToLower() == "general")
                                      .ToList();

                foreach (var noti in enviadas)
                {
                    noti.estado = "leido";
                }

                context.SaveChanges();
            }
        }


    }
}