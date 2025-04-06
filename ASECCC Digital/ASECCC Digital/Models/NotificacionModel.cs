using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASECCC_Digital.Models
{
    public class NotificacionModel
    {


        public List<Notificaciones> ObtenerNoLeidasGenerales()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Notificaciones
                              .Where(n => n.estado.ToLower() == "enviada" && n.tipo.ToLower() == "general")
                              .OrderByDescending(n => n.fechaEnvio)
                              .ToList();
            }
        }

        public List<Notificaciones> ObtenerNoLeidasPorUsuario(int usuarioId)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Notificaciones
                              .Where(n => n.estado.ToLower() == "enviada"
                                          && n.tipo.ToLower() == "personalizada"
                                          && n.usuarioId == usuarioId)
                              .OrderByDescending(n => n.fechaEnvio)
                              .ToList();
            }
        }

        public List<Notificaciones> ObtenerNotificacionesGenerales()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                return context.Notificaciones
                              .Where(n => n.tipo.ToLower() == "general")
                              .OrderByDescending(n => n.fechaEnvio)
                              .ToList();
            }
        }

        public List<Notificaciones> ObtenerNotificacionesPersonalizadas()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                int usuarioId = Convert.ToInt32(System.Web.HttpContext.Current.Session["usuarioId"]);

                return context.Notificaciones
                              .Where(n => n.usuarioId == usuarioId)
                              .OrderByDescending(n => n.fechaEnvio)
                              .ToList();
            }
        }

        public void MarcarTodasComoLeidas()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var enviadas = context.Notificaciones
                                      .Where(n => n.estado.ToLower() == "enviada" && n.tipo.ToLower() == "general")
                                      .ToList();

                foreach (var noti in enviadas)
                {
                    noti.estado = "leida";
                }

                context.SaveChanges();
            }
        }
    }

}