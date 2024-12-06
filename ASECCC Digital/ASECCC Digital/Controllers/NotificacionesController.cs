using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class NotificacionesController : Controller
    {
        //--------VISTAS ADMIN--------------//
        // GET: Notificaciones
        public ActionResult Notificacion()
        {
            return View();
        }

        public ActionResult NotificacionAdministrador()
        {
            return View();
        }

        public ActionResult NotificacionAdminGestion()
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