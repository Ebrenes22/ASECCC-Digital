using ASECCC_Digital.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASECCC_Digital.Models
{
    public class NotificacionModel
    {

        public void EnviarNotificacion(string tipo, string asunto, string mensaje, List<int> destinatarios = null)
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                var fechaActual = DateTime.Now;

                if (tipo.ToLower() == "masiva")
                {
                    var usuarios = context.Usuario.Where(u => u.estadoAfiliacion.ToLower() == "activo").ToList();

                    foreach (var usuario in usuarios)
                    {
                        var notificacion = new Notificaciones
                        {
                            usuarioId = usuario.usuarioId,
                            titulo = asunto,
                            contenido = mensaje,
                            tipo = "general",
                            fechaEnvio = fechaActual,
                            estado = "enviada"
                        };
                        context.Notificaciones.Add(notificacion);
                    }
                }
                else if (tipo.ToLower() == "personalizada" && destinatarios != null && destinatarios.Any())
                {
                    foreach (var id in destinatarios)
                    {
                        var usuario = context.Usuario.FirstOrDefault(u => u.usuarioId == id && u.estadoAfiliacion.ToLower() == "activo");

                        if (usuario != null)
                        {
                            var notificacion = new Notificaciones
                            {
                                usuarioId = usuario.usuarioId,
                                titulo = asunto,
                                contenido = mensaje,
                                tipo = "personalizada",
                                fechaEnvio = fechaActual,
                                estado = "enviada"
                            };
                            context.Notificaciones.Add(notificacion);
                        }
                    }
                }

                context.SaveChanges();
            }
        }


        public List<Notificaciones> ObtenerNoLeidasGenerales()
        {
            using (var context = new ASECCC_DIGITALEntities())
            {
                int usuarioId = Convert.ToInt32(System.Web.HttpContext.Current.Session["usuarioId"]);

                var generales = context.Notificaciones
                                      .Where(n => n.estado.ToLower() == "enviada"
                                                  && n.tipo.ToLower() == "general"
                                                  && n.usuarioId == usuarioId)
                                      .OrderByDescending(n => n.fechaEnvio)
                                      .ToList();

                return generales;
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
                int usuarioId = Convert.ToInt32(System.Web.HttpContext.Current.Session["usuarioId"]);

                var generales = context.Notificaciones
                    .Where(n => n.estado.ToLower() == "enviada" && n.tipo.ToLower() == "general" && n.usuarioId == usuarioId)
                    .ToList();

                var personalizadas = context.Notificaciones
                    .Where(n => n.estado.ToLower() == "enviada" && n.tipo.ToLower() == "personalizada" && n.usuarioId == usuarioId)
                    .ToList();

                foreach (var noti in generales.Concat(personalizadas))
                {
                    noti.estado = "leida";
                }

                context.SaveChanges();
            }
        }
    }
}