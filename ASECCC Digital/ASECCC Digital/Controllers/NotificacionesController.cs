
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
    public class NotificacionesController : Controller
    {
        UsuariosModel usuarioM = new UsuariosModel();
        NotificacionModel notificacionN = new NotificacionModel();
        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Notificaciones"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }


        //--------VISTAS ADMIN--------------//

        public ActionResult Notificaciones()
        {
            return View();
        }
        public ActionResult CrearNotificacion()
        {
            return View();
        }

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
            return View();
        }

    }
}