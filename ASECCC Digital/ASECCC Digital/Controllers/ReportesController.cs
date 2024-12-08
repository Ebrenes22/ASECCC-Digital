using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASECCC_Digital.Controllers
{
    public class ReportesController : Controller
    {

        // Acción que se ejecuta antes de cada acción del controlador
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CurrentModule = "Reportes"; //Asigno el CurrentModule para validarlo en el _MenuModulos
        }


        //--------VISTAS ADMIN--------------//
        // GET: Reportes
        public ActionResult Reporte()
        {
            return View();
        }

        public ActionResult EstadodeCuentaAsociados()
        {
            return View();
        }



        //--------VISTAS USUARIO-------------//

        public ActionResult GenerarEstadoAsociado()
        {
            return View();
        }

    }
}