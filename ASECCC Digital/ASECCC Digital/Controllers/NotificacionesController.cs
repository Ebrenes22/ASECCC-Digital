
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




        //--------VISTAS ASOCIADOS--------------//

        public ActionResult NotificacionUsuario()
        {
            var notificaciones =notificacionN.ObtenerNotificacionesPersonalizadas();
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