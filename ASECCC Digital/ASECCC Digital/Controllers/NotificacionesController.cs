
using ASECCC_Digital.Models;
using ASECCC_Digital.ViewModels;
using ASECCC_Digital.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using ASECCC_Digital.Database;
using System.Linq;
namespace ASECCC_Digital.Controllers
{
    public class NotificacionesController : BaseController
    {
        protected override string GetCurrentModule()
        {
            return "Notificaciones";
        }

        //Instancias de modelos Usuario  y Notificaciones
        private UsuariosModel usuarioM = new UsuariosModel();
        private NotificacionModel notificacionN = new NotificacionModel();


        //--------VISTAS ADMIN--------------//

        public ActionResult Notificaciones()
        {
            return View();
        }
        public ActionResult CrearNotificacion()
        {
            return View();
        }

        [Authorize]
        public ActionResult NotificacionAdministrador()
        {
            // Obtener la identificación del usuario desde la sesión
            var identificacion = Session["usuarioIdentificacion"] as string;

            if (string.IsNullOrEmpty(identificacion))
            {
                return RedirectToAction("Login", "Account"); // Si no hay sesión, redirigir al login
            }

            // Verificar si el usuario existe
            bool usuarioExiste = usuarioM.UsuarioExiste(identificacion);
            if (!usuarioExiste)
            {
                return RedirectToAction("Login", "Account"); // Redirigir si el usuario no existe
            }

            using (var context = new Database.ASECCC_DIGITALEntities()) // Se asegura de cerrar la conexión
            {
                // Buscar al usuario correctamente
                var usuario = context.Usuario.FirstOrDefault(u => u.identificacion == identificacion);

                var notificaciones = notificacionN.ObtenerNotificacionesGenerales();

                return View(notificaciones);
            }
        }

        [HttpPost]
        public JsonResult Enviar(string tipoNotificacion, string asunto, string mensaje, string destinatarios)
        {
            try
            {
                if (tipoNotificacion.Equals("masiva", StringComparison.OrdinalIgnoreCase))
                {
                    notificacionN.EnviarNotificacion("masiva", asunto, mensaje, null);
                    return Json(new { success = true, mensaje = "Notificación masiva enviada." });
                }

                if (!tipoNotificacion.Equals("personalizada", StringComparison.OrdinalIgnoreCase))
                    return Json(new { success = false, mensaje = "Tipo de notificación inválido." });

                if (string.IsNullOrWhiteSpace(destinatarios))
                    return Json(new { success = false, mensaje = "Ingrese al menos un nombre de destinatario." });

                var nombres = destinatarios
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (nombres.Count == 0)
                    return Json(new { success = false, mensaje = "No se detectaron nombres válidos." });

                using (var context = new ASECCC_DIGITALEntities())
                {
                    var usuarios = context.Usuario
                        .Where(u => u.estadoAfiliacion.ToLower() == "activo")
                        .Where(u => nombres.Contains(u.nombreCompleto))
                        .Select(u => new { u.usuarioId, u.nombreCompleto })
                        .ToList();

                    var ids = usuarios.Select(u => u.usuarioId).ToList();
                    if (ids.Count == 0)
                        return Json(new { success = false, mensaje = "No se encontraron usuarios activos con esos nombres." });

                    notificacionN.EnviarNotificacion("personalizada", asunto, mensaje, ids);

                    var nombresEncontrados = usuarios.Select(u => u.nombreCompleto)
                                                     .Distinct(StringComparer.OrdinalIgnoreCase)
                                                     .ToList();
                    var noEncontrados = nombres.Except(nombresEncontrados, StringComparer.OrdinalIgnoreCase).ToList();

                    var msg = $"Notificación enviada a {ids.Count} usuario(s).";
                    if (noEncontrados.Count > 0)
                        msg += $" No encontrados: {string.Join(", ", noEncontrados)}.";

                    return Json(new { success = true, mensaje = msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error al enviar la notificación: " + ex.Message });
            }
        }




        //--------VISTAS ASOCIADOS--------------//

        public ActionResult NotificacionUsuario()
        {
            var notificaciones = notificacionN.ObtenerNotificacionesPersonalizadas();
            return View(notificaciones);
        }

        [HttpGet]
        public JsonResult ObtenerNotificacionesNoLeidas()
        {
            try
            {
                int usuarioId = Convert.ToInt32(Session["usuarioId"]);
                var modelo = new NotificacionModel();

                var generales = modelo.ObtenerNoLeidasGenerales();
                var personalizadas = modelo.ObtenerNoLeidasPorUsuario(usuarioId);

                var resultado = generales
                    .Concat(personalizadas)
                    .OrderByDescending(n => n.fechaEnvio)
                    .Select(n => new
                    {
                        n.notificacionId,
                        n.titulo,
                        fecha = n.fechaEnvio.HasValue ? n.fechaEnvio.Value.ToString("dd-MM-yyyy HH:mm") : ""
                    })
                    .ToList();

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult MarcarTodasComoLeidas()
        {
            try
            {
                notificacionN.MarcarTodasComoLeidas();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult MarcarComoLeida(int id)
        {
            try
            {
                using (var context = new ASECCC_DIGITALEntities())
                {
                    var notificacion = context.Notificaciones.FirstOrDefault(n => n.notificacionId == id);
                    if (notificacion != null)
                    {
                        notificacion.estado = "leida";
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}