using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class PrestamosController : Controller
    {
        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Prestamos"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }
        //--------VISTAS ADMIN--------------//

        // GET: Prestamos
        public ActionResult Prestamo()
        {
            return View();
        }

        public ActionResult RegistrarAbonos()
        {

            return View();
        }

        public ActionResult ConsultaPrestamosAdmin()
        {
            return View();
        }

        public ActionResult RevisionPrestamos()
        {
            return View();
        }

        //----------VISTAS ASOCIADO-----------//

        public ActionResult SolicitudPrestamo()
        {
            return View();
        }

        public ActionResult ConsultaPrestamoAsociado()
        {
            return View();
        }
    }
}