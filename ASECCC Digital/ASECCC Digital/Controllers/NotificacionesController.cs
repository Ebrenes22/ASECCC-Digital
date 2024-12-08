using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class NotificacionesController : Controller
    {

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
            return View();
        }

        //--------VISTAS ASOCIADOS--------------//

        public ActionResult NotificacionUsuario()
        {
            return View();
        }

    }
}